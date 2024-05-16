using System.Collections.Generic;
using DialogueSystemTool.Data;
using DialogueSystemTool.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private List<BaseNode> nodes;
        private List<NodeConnection> connections;
        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private BaseNode selectedNode;
        private DialogueTree dialogueTree;

        private DialogueNodeEditor selectedOutNodeEditor;
        private int selectedOutOptionIndex;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Editor");
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            nodes = new List<BaseNode>();
            connections = new List<NodeConnection>();
        }

        private void OnGUI()
        {
            DrawToolbar();

            DrawNodes();
            DrawConnections();
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Save Dialogue Tree", EditorStyles.toolbarButton))
            {
                SaveDialogueTree();
            }

            if (GUILayout.Button("Load Dialogue Tree", EditorStyles.toolbarButton))
            {
                LoadDialogueTree();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                foreach (var connection in connections)
                {
                    connection.Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ShowContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.MouseUp:
                    if (selectedOutNodeEditor != null && selectedNode != null && selectedNode is DialogueNodeEditor)
                    {
                        CreateConnection(selectedOutNodeEditor, (DialogueNodeEditor)selectedNode, selectedOutOptionIndex);
                    }
                    ClearConnectionSelection();
                    break;
                case EventType.MouseDrag:
                    if (selectedOutNodeEditor != null)
                    {
                        DragConnection(e.mousePosition);
                    }
                    break;
            }
        }

        private void ShowContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Dialogue Node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            DialogueNode dataNode = new DialogueNode(); // Create a new data node
            var dialogueNodeEditor = new DialogueNodeEditor(mousePosition, 200, 100, nodeStyle, selectedNodeStyle, dataNode);
            dialogueNodeEditor.OnRemoveNode += OnRemoveNode; // Subscribe to the remove event
            nodes.Add(dialogueNodeEditor);
            if (dialogueTree != null)
            {
                dialogueTree.nodes.Add(dataNode);
            }
        }

        private void OnRemoveNode(BaseNode node)
        {
            if (connections != null)
            {
                List<NodeConnection> connectionsToRemove = new List<NodeConnection>();

                foreach (var connection in connections)
                {
                    if (connection.startNode == node || connection.endNode == node)
                    {
                        connectionsToRemove.Add(connection);
                    }
                }

                foreach (var connection in connectionsToRemove)
                {
                    connections.Remove(connection);
                }
            }

            nodes.Remove(node);
            var dialogueNodeEditor = node as DialogueNodeEditor;
            if (dialogueNodeEditor != null && dialogueTree != null)
            {
                dialogueTree.nodes.Remove(dialogueNodeEditor.dataNode);
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);
                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void SaveDialogueTree()
        {
            if (dialogueTree != null)
            {
                EditorUtility.SetDirty(dialogueTree);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void LoadDialogueTree()
        {
            string path = EditorUtility.OpenFilePanel("Load Dialogue Tree", "Assets", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                dialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
                if (dialogueTree != null)
                {
                    nodes.Clear();
                    connections.Clear();
                    foreach (var dataNode in dialogueTree.nodes)
                    {
                        var dialogueNodeEditor = new DialogueNodeEditor(Vector2.zero, 200, 100, nodeStyle, selectedNodeStyle, dataNode);
                        dialogueNodeEditor.OnRemoveNode += OnRemoveNode; // Subscribe to the remove event
                        nodes.Add(dialogueNodeEditor);
                    }
                }
            }
        }

        private void CreateConnection(DialogueNodeEditor outNode, DialogueNodeEditor inNode, int optionIndex)
        {
            var connection = new NodeConnection(outNode, inNode);
            connections.Add(connection);

            // Set the target node ID for the option
            outNode.dataNode.options[optionIndex].nextNodeId = inNode.dataNode.id;
        }

        private void ClearConnectionSelection()
        {
            selectedOutNodeEditor = null;
            selectedOutOptionIndex = -1;
        }

        private void DragConnection(Vector2 mousePosition)
        {
            Handles.DrawBezier(
                selectedOutNodeEditor.rect.center,
                mousePosition,
                selectedOutNodeEditor.rect.center + Vector2.left * 50f,
                mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
            GUI.changed = true;
        }

        public void SetOptionTargetNode(DialogueNodeEditor nodeEditor, int optionIndex)
        {
            selectedOutNodeEditor = nodeEditor;
            selectedOutOptionIndex = optionIndex;
        }
    }
}