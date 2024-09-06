 using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private CutsceneTimeline _timeline;
        [SerializeField] private bool _skippable;

        private CutsceneInfo _cutsceneInfo;

        private void Start()
        {
            _cutsceneInfo = GameContext.Context.CutsceneInfo;
        }

        public void PlayCutscene()
        {
            _cutsceneInfo.StartCutscene(_timeline, _skippable);
        }
    }
}