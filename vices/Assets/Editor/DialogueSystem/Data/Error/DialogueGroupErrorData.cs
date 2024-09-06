using JabberwockyWorld.DialogueSystem.Editor.Elements;
using System.Collections.Generic;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    public class DialogueGroupErrorData
    {
        public DialogueErrorData Error { get; set; }
        public List<GraphGroup> Groups { get; set; }
        
        public DialogueGroupErrorData()
        {
            Error = new DialogueErrorData();
            Groups = new List<GraphGroup>();
        }
    }
}
