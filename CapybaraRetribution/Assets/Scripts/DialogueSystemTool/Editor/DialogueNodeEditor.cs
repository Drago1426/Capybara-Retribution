using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogueSystemTool.Data;

namespace DialogueSystemTool.Editor
{
    /// <summary>
    /// DialogueNodeEditor handles the graphical representation and editing of DialogueNode objects in the editor.
    /// </summary>
    public class DialogueNodeEditor : BaseNode
    {
        public DialogueNode dataNode; // Reference to the data node
        private List<Rect> optionRects; // Rectangles for option connection points

        /// <summary>
        /// Constructor for DialogueNodeEditor.
        /// </summary>
        /// <param name="position">Position of the node.</param>
        /// <param name="width">Width of the node.</param>
        /// <param name="height">Height of the node.</param>
        /// <param name="nodeStyle">GUIStyle for the node.</param>
        /// <param name="selectedStyle">GUIStyle for the selected node.</param>
        /// <param name="dataNode">Reference to the DialogueNode data.</param>
        public DialogueNodeEditor(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, DialogueNode dataNode)
            : base(position, width, height, nodeStyle, selectedStyle)
        {
            this.dataNode = dataNode;
            title = "Dialogue Node";
            optionRects = new List<Rect>();
            UpdateHeight();
        }

        /// <summary>
        /// Draws the DialogueNodeEditor in the editor window.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 25, rect.width - 20, rect.height - 25));

            GUILayout.Label("Dialogue Text:", EditorStyles.label);
            dataNode.dialogueText = EditorGUILayout.TextArea(dataNode.dialogueText, GUILayout.Height(60));

            GUILayout.Space(10);

            GUILayout.Label("Options:", EditorStyles.label);
            optionRects.Clear();
            for (int i = 0; i < dataNode.options.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                dataNode.options[i].optionText = EditorGUILayout.TextField(dataNode.options[i].optionText);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    dataNode.options.RemoveAt(i);
                    UpdateHeight();
                    break; // Exit the loop after modification to avoid layout issues
                }

                // Display the button to set the target node for this option
                if (GUILayout.Button("O", GUILayout.Width(20)))
                {
                    DialogueEditorWindow editorWindow = (DialogueEditorWindow)EditorWindow.GetWindow(typeof(DialogueEditorWindow));
                    editorWindow.SetOptionTargetNode(this, i);
                }

                EditorGUILayout.EndHorizontal();

                // Create a rect for the connection point
                Rect optionRect = GUILayoutUtility.GetLastRect();
                optionRects.Add(optionRect);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Add Option"))
            {
                dataNode.options.Add(new DialogueOption());
                UpdateHeight();
            }

            GUILayout.EndArea();

            // Context menu for removing the node
            Event e = Event.current;
            if (e.type == EventType.ContextClick && rect.Contains(e.mousePosition))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Remove Node"), false, () => RemoveNode());
                menu.ShowAsContext();
                e.Use();
            }
        }

        /// <summary>
        /// Gets the rectangle for the option at the specified index.
        /// </summary>
        /// <param name="index">Index of the option.</param>
        /// <returns>Rectangle for the option.</returns>
        public Rect GetOptionRect(int index)
        {
            if (index < optionRects.Count)
            {
                return optionRects[index];
            }
            return new Rect();
        }

        /// <summary>
        /// Gets the center right position of the rectangle for the option at the specified index.
        /// </summary>
        /// <param name="index">Index of the option.</param>
        /// <returns>Center right position of the rectangle.</returns>
        public Vector2 GetOptionRectCenterRight(int index)
        {
            if (index < optionRects.Count)
            {
                Rect optionRect = optionRects[index];
                return new Vector2(optionRect.xMax, optionRect.y + optionRect.height * 0.5f);
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Updates the height of the node based on the number of options.
        /// </summary>
        private void UpdateHeight()
        {
            rect.height = 180 + (dataNode.options.Count * 35); // Adjust height based on options
        }
    }
}
