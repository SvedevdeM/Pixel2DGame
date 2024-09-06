using System;
using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [Serializable]
    public class ControlDriverNew
    {
        [SerializeField] protected string name;
        [SerializeField] protected Controll controll;
        [SerializeField] protected bool autoIncrement;
        [SerializeField] protected bool percentageBased;
        [HideInInspector][SerializeField] protected string guid = Guid.NewGuid().ToString();

        public ControlDriverNew()
        {
        }

        /*public ControlDriverNew(string name = null, bool autoIncrement = false, bool percentageBased = false)
        {
            this.name = name;
            this.autoIncrement = autoIncrement;
            this.percentageBased = percentageBased;
        }*/

        public ControlDriverNew(bool autoIncrement)
        {
            this.autoIncrement = autoIncrement;
        }

        public void ChangeControl (Controll controll)
        {
            this.controll = controll;
        }

        public int ResolveDriver(IReadOnlyAnimatorState previousState, AnimatorState nextState, int size)
        {
            if (size == 0) return 0;
            string driverName = guid;
            if (controll != null)
            {
                driverName = string.IsNullOrEmpty(controll.Name) ? guid : controll.Name;
            }


            int driverValue = previousState.Get(driverName) % size;
            if (autoIncrement)
                nextState.Set(driverName, (driverValue + 1) % size);

            return driverValue;
        }
    }
}