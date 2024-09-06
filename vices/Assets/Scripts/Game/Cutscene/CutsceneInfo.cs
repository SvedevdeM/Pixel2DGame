using System;
using UnityEngine;

namespace Vices.Scripts.Game
{
    [CreateAssetMenu(fileName = nameof(CutsceneInfo), menuName = "Vices/" + nameof(CutsceneInfo))]
    public class CutsceneInfo : ScriptableObject
    {
        private Action<CutsceneTimeline, bool> _onCutsceneStarted;

        public void StartCutscene(CutsceneTimeline timeline, bool isSkippable)
        {
            _onCutsceneStarted?.Invoke(timeline, isSkippable);
        }

        public void SubscribeOnCutsceneStart(Action<CutsceneTimeline, bool> onCutsceneStarted)
        {
            _onCutsceneStarted += onCutsceneStarted;
        }

        public void UnsubscribeOnCutsceneStart(Action<CutsceneTimeline, bool> onCutsceneStarted)
        {
            _onCutsceneStarted -= onCutsceneStarted;
        }
    }
}