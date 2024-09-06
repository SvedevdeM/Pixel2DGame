using UnityEngine;

using JabberwockyWorld.DynamicEvents.Scripts.Structs;


namespace JabberwockyWorld.DynamicEvents.Scripts
{
    public class FactField_DE : MonoBehaviour
    {
        public FactField FactField;

        public void Start()
        {
            FactField.Initialize();
        }

        public void IncreaseOne(int i)
        {
            FactField.Initialize();
            FactField.FactEntry.Value += i;
        }
    }
}