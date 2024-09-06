using JabberwockyWorld.DynamicEvents.Scripts.Structs;
using UnityEngine;

namespace Vices.Scripts
{
    public class EventInteractable : Interactable
    {
        [SerializeField] private EventField _onInteractEvent;

        protected override void Start()
        {
            base.Start();
            _onInteractEvent.Initialize();
        }

        protected override void OnEnter(Collider other) { }

        protected override void OnInteract()
        {
            _onInteractEvent.TriggerEvent();
            _canInteract = false;
        }
    }
}