using UnityEditor;
using UnityEngine;
using DialogueSystemTool.Data;
using DialogueSystemTool.ScriptableObjects;

namespace DialogueSystemTool.Editor
{
    /// <summary>
    /// Custom editor for the DialogueTree ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(DialogueTree))]
    public class DialogueTreeEditor : UnityEditor.Editor
    {
        private SerializedProperty nodesProp;

        private void OnEnable()
        {
            // Find the serialized property "nodes" in the DialogueTree script
            nodesProp = serializedObject.FindProperty("nodes");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object representation
            serializedObject.Update();

            // Display the "nodes" property field in the Inspector
            EditorGUILayout.PropertyField(nodesProp, new GUIContent("Dialogue Nodes"), true);

            // Button to add a new node
            if (GUILayout.Button("Add Node"))
            {
                AddNode();
            }

            // Button to remove the last node
            if (GUILayout.Button("Remove Last Node"))
            {
                RemoveNode();
            }

            // Apply the modified properties to the serialized object
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Adds a new DialogueNode to the DialogueTree.
        /// </summary>
        private void AddNode()
        {
            var tree = (DialogueTree)target;
            var newNode = new DialogueNode();
            tree.nodes.Add(newNode);
            EditorUtility.SetDirty(target);
        }

        /// <summary>
        /// Removes the last DialogueNode from the DialogueTree.
        /// </summary>
        private void RemoveNode()
        {
            var tree = (DialogueTree)target;
            if (tree.nodes.Count > 0)
            {
                tree.nodes.RemoveAt(tree.nodes.Count - 1);
                EditorUtility.SetDirty(target);
            }
        }
    }
}
