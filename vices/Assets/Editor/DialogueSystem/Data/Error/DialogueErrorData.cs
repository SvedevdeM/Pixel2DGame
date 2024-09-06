using UnityEngine;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    public class DialogueErrorData
    {
        public Color Color { get; private set; }

        public DialogueErrorData()
        {
            GenerateErrorColor();
        }

        private void GenerateErrorColor()
        {
            Color = new Color32((byte)Random.Range(65, 256), (byte)Random.Range(50, 176), (byte)Random.Range(50, 176), 255);
        }
    }
}
