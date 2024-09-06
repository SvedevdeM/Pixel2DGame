using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using JabberwockyWorld.Quest.Scripts;
using UnityEngine;
using Vices.Scripts.Core;
using Vices.Scripts.Game;
using static JabberwockyWorld.DynamicEvents.Scripts.Criteria;

namespace Vices.Scripts
{
    public class CutsceneInteractable : Interactable
    {
        [SerializeField] private Cutscene _cutscene;
        [SerializeField] private FactField _fact;
        [SerializeField] private EqualType _equalType;
        [SerializeField] private int _value;
        [SerializeField] private bool _once;

        private CutscenesInfo _cutsceneInfo;
        private bool _isPlayedOnce;

        protected override void Start()
        {
            base.Start();

            _isNeedUi = false;
            if (_fact.FactEntry != null) _fact.Initialize();

            if (!_once) return;
            _cutsceneInfo = GameContext.Context.CutscenesInfo;
            _isPlayedOnce = _cutsceneInfo.CheckSetting(gameObject.name);
        }

        protected override void OnEnter(Collider other) 
        {
            Debug.Log(_isPlayedOnce);
            if (_once && _fact.FactEntry == null)
            {
                _cutsceneInfo = GameContext.Context.CutscenesInfo;
                _isPlayedOnce = _cutsceneInfo.CheckSetting(gameObject.name);
                Debug.Log("A");
                Debug.Log(_isPlayedOnce);
                if (_isPlayedOnce) return;               
            }

            if (_isPlayedOnce) return;
            if (_fact.FactEntry == null) return;
            _fact.Initialize();
            
            if (!CheckCriteria(_fact.FactEntry, _value, _equalType)) return;
            
            _cutscene.PlayCutscene();
            if (!_once) return;
            _isPlayedOnce = true;
           _cutsceneInfo.SetSettting(gameObject.name, _isPlayedOnce);
        }

        protected override void OnInteract() { }

        private bool CheckCriteria(FactEntry fact, float value, EqualType type)
        {
            switch (type)
            {
                case EqualType.None:
                    return false;
                case EqualType.Greater:
                    if (fact.Value > value) return true;
                    break;
                case EqualType.GreaterOrEqual:
                    if (fact.Value >= value) return true;
                    break;
                case EqualType.Less:
                    if (fact.Value < value) return true;
                    break;
                case EqualType.LessOrEqual:
                    if (fact.Value <= value) return true;
                    break;
                case EqualType.Equal:
                    if (fact.Value == value) return true;
                    break;
                default:
                    return false;
            }

            return false;
        }
    }
}