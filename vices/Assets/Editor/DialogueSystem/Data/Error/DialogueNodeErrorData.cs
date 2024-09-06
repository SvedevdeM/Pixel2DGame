using JabberwockyWorld.DialogueSystem.Editor.Elements;
using System.Collections.Generic;

namespace JabberwockyWorld.DialogueSystem.Editor.Data
{
    public class DialogueNodeErrorData
    {
        public DialogueErrorData Error { get; set; }
        public List<GraphNode> Nodes { get; set; }

        public DialogueNodeErrorData()
        {
            Error = new DialogueErrorData();
            Nodes = new List<GraphNode>();
        }
    }
}
