using System;

namespace JabberwockyWorld.DialogueSystem.Scripts.Data
{
    [Serializable]
    public class DialogueChoiceRuntimeData
    {
        public string Text;
        public DialogueRuntimeData NextDialogue;
    }
}
