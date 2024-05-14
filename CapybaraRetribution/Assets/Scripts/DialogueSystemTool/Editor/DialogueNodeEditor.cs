using UnityEngine;
using UnityEditor;
using DialogueSystemTool.Data;

namespace DialogueSystemTool.Editor
{
    public class DialogueNodeEditor : BaseNode
    {
        public DialogueNode dataNode; // Reference to the data node

        public DialogueNodeEditor(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, DialogueNode dataNode)
            : base(position, width, height, nodeStyle, selectedStyle)
        {
            this.dataNode = dataNode;
            title = "Dialogue Node";
            UpdateHeight();
        }

        public override void Draw()
        {
            base.Draw();

            GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 25, rect.width - 20, rect.height - 25));

            GUILayout.Label("Dialogue Text:", EditorStyles.label);
            dataNode.dialogueText = EditorGUILayout.TextArea(dataNode.dialogueText, GUILayout.Height(60));

            GUILayout.Space(10);

            GUILayout.Label("Options:", EditorStyles.label);
            for (int i = 0; i < dataNode.options.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                dataNode.options[i].optionText = EditorGUILayout.TextField(dataNode.options[i].optionText, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    dataNode.options.RemoveAt(i);
                    UpdateHeight();
                    break; // Exit the loop after modification to avoid layout issues
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Add Option"))
            {
                dataNode.options.Add(new DialogueOption());
                UpdateHeight();
            }

            GUILayout.EndArea();
        }

        private void UpdateHeight()
        {
            rect.height = 140 + (dataNode.options.Count * 25); // Adjust height based on options
        }
    }
}