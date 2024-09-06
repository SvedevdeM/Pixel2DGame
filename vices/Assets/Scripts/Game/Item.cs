using UnityEngine;

namespace Vices.Scripts
{
    [CreateAssetMenu(fileName = nameof(Item), menuName = "JabberwockyWorld/Game/" + nameof(Item))]
    public class Item : ScriptableObject
    {
        public Sprite Sprite;
        public string Name;
        public string Description;
        public int Amount;
    }
}
