using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using UnityEngine;

namespace JabberwockyWorld.DynamicEvents.Scripts
{
    [CreateAssetMenu(fileName = nameof(EventData), menuName = "JabberwockyWorld/DynamicEvents/" + nameof(EventData))]
    public class EventData : ScriptableObject
    {
        public EventField Event;

        public void Execute()
        {
            Event.Initialize();
            Event.TriggerEvent();
        }
    }
}