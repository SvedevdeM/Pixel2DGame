using UnityEngine;
using UnityEngine.Events;

using JabberwockyWorld.DynamicEvents.Scripts.Structs;


public class SubscribeOnEventField_DE : MonoBehaviour 
{
    public UnityEvent OnEvent;
    public EventField EventField;

    private void Start()
    {
        EventField.Initialize();
        EventField.SubscribeOnEvent(() => OnEvent?.Invoke());
    }
}