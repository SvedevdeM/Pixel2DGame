using UnityEngine;

namespace JabberwockyWorld.AnimationSystem.Editor.Data
{
    public class AnimationErrorData
    {
        public Color Color { get; private set; }

        public AnimationErrorData()
        {
            GenerateErrorColor();
        }

        private void GenerateErrorColor()
        {
            Color = new Color32((byte)Random.Range(65, 256), (byte)Random.Range(50, 176), (byte)Random.Range(50, 176), 255);
        }
    }
}
