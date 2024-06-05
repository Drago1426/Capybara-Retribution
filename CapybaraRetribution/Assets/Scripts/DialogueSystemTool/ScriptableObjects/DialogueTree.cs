using DialogueSystemTool.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystemTool.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DialogueTree", menuName = "Dialogue/DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        public List<DialogueNode> nodes = new List<DialogueNode>(); // List of all nodes in the dialogue tree

        public DialogueNode GetNodeById(string id)
        {
            return nodes.Find(node => node.id == id);
        }
    }
}