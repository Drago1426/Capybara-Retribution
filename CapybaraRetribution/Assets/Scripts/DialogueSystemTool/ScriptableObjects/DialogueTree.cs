using DialogueSystemTool.Data;

namespace DialogueSystemTool.ScriptableObjects
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "DialogueTree", menuName = "Dialogue/DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        public List<DialogueNode> nodes; // List of all nodes in the dialogue tree
    }
}