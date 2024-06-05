using System.Collections.Generic;

namespace DialogueSystemTool.Data
{
    /// <summary>
    /// Represents an option in a dialogue node.
    /// </summary>
    [System.Serializable]
    public class DialogueOption
    {
        public string optionText; // Text displayed for this option
        public string nextNodeId; // ID of the next node
    }
}