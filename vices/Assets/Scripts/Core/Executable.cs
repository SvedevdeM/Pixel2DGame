using UnityEngine;

namespace Vices.Scripts.Core
{
    public abstract class Executable : MonoBehaviour, IExecutable
    {
        public abstract void Execute();

        public virtual void Start()
        {
            GameManager.Singleton.AddExecutable(this);
        }

        public virtual void OnDestroy()
        {
            GameManager.Singleton.RemoveExecutable(this);
        }
    }
}