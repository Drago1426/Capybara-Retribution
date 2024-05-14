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
        }

        public override void Draw()
        {
            base.Draw();

            Rect textRect = new Rect(rect.x + 10, rect.y + 30, rect.width - 20, rect.height - 40);
            dataNode.dialogueText = EditorGUI.TextField(textRect, dataNode.dialogueText);

            // Draw dialogue options
            for (int i = 0; i < dataNode.options.Count; i++)
            {
                DialogueOption option = dataNode.options[i];
                Rect optionRect = new Rect(rect.x + 10, rect.y + 60 + (i * 25), rect.width - 20, 20);
                option.optionText = EditorGUI.TextField(optionRect, option.optionText);
            }

            // Add button to add new option
            if (GUI.Button(new Rect(rect.x + 10, rect.y + rect.height - 30, rect.width - 20, 20), "Add Option"))
            {
                dataNode.options.Add(new DialogueOption());
            }
        }
    }
}