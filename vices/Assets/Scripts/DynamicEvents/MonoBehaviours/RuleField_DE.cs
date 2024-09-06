using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Structs;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    public class RuleField_DE : MonoBehaviour
    {
        public RuleField RuleField;

        public void Awake()
        {
            RuleField.Initialize();
        }
    }
}