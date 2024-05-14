using UnityEditor;
using UnityEngine;
using DialogueSystemTool.Data;
using DialogueSystemTool.ScriptableObjects;

namespace DialogueSystemTool.Editor
{
    
    [CustomEditor(typeof(DialogueTree))]
    public class DialogueTreeEditor : UnityEditor.Editor
    {
        private SerializedProperty nodesProp;

        private void OnEnable()
        {
            nodesProp = serializedObject.FindProperty("nodes");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(nodesProp, new GUIContent("Dialogue Nodes"), true);

            if (GUILayout.Button("Add Node"))
            {
                AddNode();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddNode()
        {
            var tree = (DialogueTree)target;
            var newNode = new DialogueNode();
            tree.nodes.Add(newNode);
            EditorUtility.SetDirty(target);
        }
    }
}