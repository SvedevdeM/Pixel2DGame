using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vices.Scripts.Core
{
    public class SceneSystem : MonoBehaviour
    {
        [SerializeField] private SceneData[] _sceneDatas;
        private LoadScreen _loadScreen;

        private Dictionary<string, SceneData> _sceneDatasDictionary;
        public List<string> _scenesToLoad;
        private SceneData _sceneData;

        private Action _onSceneLoaded;

        private string _sceneName;

        public static SceneSystem Singleton { get; private set; }

        public void Awake()
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }

        public void Start()
        {
            _scenesToLoad = new List<string>();
            _sceneDatasDictionary = new Dictionary<string, SceneData>();
            for (int i = 0; i < _sceneDatas.Length; i++)
            {
                _sceneDatasDictionary.Add(_sceneDatas[i].Name, _sceneDatas[i]);
            }

            _loadScreen = ObjectUtils.CreateGameObject<LoadScreen>(GameContext.Context.AssetsContext.GetObjectOfType(typeof(GameObject), "LoadCanvas") as GameObject);
        }

        public void LoadScene(string name, Action onSceneLoaded = null)
        {
            GameManager.Singleton.ClearAllExecutables();
            GameContext.Context.InteractablePool.RefreshPool();

            _sceneDatasDictionary.TryGetValue(name, out _sceneData);

            _sceneName = name;
            _onSceneLoaded = onSceneLoaded;
            _scenesToLoad.Clear();

            SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive).completed += LoadingScene;
        }

        private void LoadingScene(AsyncOperation asyncOperation)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Loading"));

            _loadScreen.Show(() =>
            {
                _scenesToLoad.Add(_sceneName);
                for (int i = 0; i < _sceneData.Scenes.Length; i++)
                {
                    _scenesToLoad.Add(_sceneData.Scenes[i].name);
                }

                StartCoroutine(ManageScenes());;
            });
        }

        private IEnumerator ManageScenes()
        {
            yield return StartCoroutine(UnloadAllActiveScenes());

            yield return StartCoroutine(LoadScenes());

           OnSceneComplete();
        }

        private IEnumerator UnloadAllActiveScenes()
        {
            List<AsyncOperation> unloadOperations = new List<AsyncOperation>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && scene.name != SceneManager.GetActiveScene().name)
                {
                    AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);
                    unloadOperations.Add(asyncUnload);
                }
            }

            foreach (AsyncOperation operation in unloadOperations)
            {
                while (!operation.isDone)
                {
                    yield return null;
                }
            }
        }

        private IEnumerator LoadScenes()
        {
            List<AsyncOperation> loadOperations = new List<AsyncOperation>();

            foreach (string sceneName in _scenesToLoad)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                loadOperations.Add(asyncLoad);
            }

            foreach (AsyncOperation operation in loadOperations)
            {
                while (!operation.isDone)
                {
                    yield return null;
                }
            }
        }


        private void OnSceneComplete()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));

            _onSceneLoaded?.Invoke();
            _loadScreen.Hide(() =>
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Loading"));
            });
        }
    }
}