using System.Collections;
using UnityEngine;
using Vices.Scripts.Core;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private bool _initialize = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        if (_initialize) LoadScene(_sceneName);
    }

    private void LoadScene(string name)
    {
        GameContext.Context.PreviousScene = name;

        SceneSystem.Singleton.LoadScene(name);
    }
}
