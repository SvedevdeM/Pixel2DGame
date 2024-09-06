using UnityEngine;

namespace Vices.Scripts.Game
{
    public class PlayerUI : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}