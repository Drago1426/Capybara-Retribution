using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystemTool.Data
{
    [Serializable]
    public class DialogueNode
    {
        public string id; // Unique identifier for the node
        [TextArea(3, 10)] public string dialogueText; // The dialogue text
        public List<DialogueOption> options = new List<DialogueOption>(); // List of options branching from this node

        public DialogueNode()
        {
            id = Guid.NewGuid().ToString(); // Generate a unique ID
        }
    }
}