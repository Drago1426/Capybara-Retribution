using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystemTool.Data
{
    /// <summary>
    /// Represents a node in the dialogue tree.
    /// </summary>
    [System.Serializable]
    public class DialogueNode
    {
        public string id; // Unique identifier for the node
        [TextArea(3, 10)] public string dialogueText; // The dialogue text
        public List<DialogueOption> options = new List<DialogueOption>(); // List of options branching from this node

        /// <summary>
        /// Constructor for the DialogueNode class.
        /// </summary>
        public DialogueNode()
        {
            id = System.Guid.NewGuid().ToString(); // Generate a unique ID
        }
    }
}

