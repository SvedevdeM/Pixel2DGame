using System;
using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Structs;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    public class EventField_DE : MonoBehaviour
    {
        [SerializeField] private EventField EventField;

        private void Start()
        {
            Debug.Log($"Initialzing {typeof(EventField)} in {this}");
            EventField.Initialize();
        }

        public void StartEvent()
        {
            EventField.Initialize();
            EventField.TriggerEvent();
        }

        public void SubscribeOnEvent(Action action = null)
        {
            EventField.Initialize();
            EventField.SubscribeOnEvent(action);
        }
    }
}