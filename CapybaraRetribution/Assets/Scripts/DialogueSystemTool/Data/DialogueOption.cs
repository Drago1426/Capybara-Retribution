using System.Collections.Generic;

namespace DialogueSystemTool.Data
{
    [System.Serializable]
    public class DialogueOption
    {
        public string optionText; // Text displayed for this option
        public string nextNodeId; // ID of the next node to go to if this option is chosen
        public List<Condition> conditions = new List<Condition>(); // Conditions required for this option to be available
    }
}