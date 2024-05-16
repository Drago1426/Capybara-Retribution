using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Data
{
    public class BaseNode
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        public delegate void NodeEvent(BaseNode node);
        public event NodeEvent OnRemoveNode;

        public BaseNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }

        public virtual void Draw()
        {
            GUI.Box(rect, "", style); // Draw an empty box to set the style
            GUI.Label(new Rect(rect.x + 10, rect.y + 5, rect.width - 20, 20), title, EditorStyles.boldLabel); // Draw the title inside the box
        }

        public virtual void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && isSelected)
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            OnRemoveNode?.Invoke(this);
        }

        public Vector2 GetConnectionPoint()
        {
            return new Vector2(rect.xMax, rect.center.y);
        }
    }
}