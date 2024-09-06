using JabberwockyWorld.DynamicEvents.Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EventDatabase _database;
        [SerializeField] private GameConfig _config;

        public static GameManager Singleton;

        private List<IExecutable> _executables = new List<IExecutable>();

        private void Awake()
        {
            if (Singleton == null) Singleton = this;
            new GameContext(_config);
        }

        private void Start()
        {
            new LocalizationReader(_config.Localization);
            _config.CutscenesInfo.Initialize();

            if (_database != null)
            {
                _database.InitializeEntries();
                _database.RuleEntries.Clear();              
            }
        }

        private void Update()
        {
            for (int i = 0; i < _executables.Count; i++)
            {
                _executables[i].Execute();
            }
            _database.Execute();
        }

        public void AddExecutable(IExecutable executable)
        {
            _executables.Add(executable);
        }

        public void ClearAllExecutables()
        {
            _executables.Clear();
        }

        public void RemoveExecutable(IExecutable executable)
        {
            _executables.Remove(executable);
        }
    }
}
