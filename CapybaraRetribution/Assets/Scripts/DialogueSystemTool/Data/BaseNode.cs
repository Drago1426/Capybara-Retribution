using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Data
{
    /// <summary>
    /// Base class for a node in the dialogue system.
    /// </summary>
    public abstract class BaseNode
    {
        public Rect rect; // The rectangle defining the node's position and size
        public string title; // The title of the node
        private bool isSelected; // Whether the node is selected
        private GUIStyle nodeStyle; // Style for the node
        private GUIStyle selectedNodeStyle; // Style for the selected node
        private Action<BaseNode> OnRemoveNode; // Callback for removing the node

        /// <summary>
        /// Constructor for the BaseNode class.
        /// </summary>
        /// <param name="position">Position of the node.</param>
        /// <param name="width">Width of the node.</param>
        /// <param name="height">Height of the node.</param>
        /// <param name="nodeStyle">Style for the node.</param>
        /// <param name="selectedNodeStyle">Style for the selected node.</param>
        public BaseNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedNodeStyle)
        {
            rect = new Rect(position.x, position.y, width, height);
            this.nodeStyle = nodeStyle;
            this.selectedNodeStyle = selectedNodeStyle;
        }

        /// <summary>
        /// Draws the node.
        /// </summary>
        public virtual void Draw()
        {
            GUI.Box(rect, title, isSelected ? selectedNodeStyle : nodeStyle);
        }

        /// <summary>
        /// Processes events for the node.
        /// </summary>
        /// <param name="e">The event to process.</param>
        /// <returns>True if the event was used, false otherwise.</returns>
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isSelected = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            isSelected = false;
                            GUI.changed = true;
                        }
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isSelected)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// Drags the node by a given delta.
        /// </summary>
        /// <param name="delta">The amount to drag the node.</param>
        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        /// <summary>
        /// Sets the callback for removing the node.
        /// </summary>
        /// <param name="onRemoveNode">The callback to set.</param>
        public void SetOnRemoveNode(Action<BaseNode> onRemoveNode)
        {
            OnRemoveNode = onRemoveNode;
        }

        /// <summary>
        /// Invokes the remove node callback.
        /// </summary>
        public void RemoveNode()
        {
            OnRemoveNode?.Invoke(this);
        }
    }
}
