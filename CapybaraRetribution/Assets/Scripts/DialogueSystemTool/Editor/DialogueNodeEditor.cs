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
        }
    }
}