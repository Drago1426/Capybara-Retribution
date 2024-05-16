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
        private int selectedOutOptionIndex = -1;

        // Create menu item to show the dialogue editor window
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Editor");
        }

        private void OnEnable()
        {
            // Load styles for nodes
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

            if (GUILayout.Button("Add Node", EditorStyles.toolbarButton))
            {
                AddNode(new Vector2(10, 10)); // Default position for the new node
            }

            if (GUILayout.Button("Remove Selected Node", EditorStyles.toolbarButton))
            {
                RemoveSelectedNode();
            }

            GUILayout.EndHorizontal();
        }

        private void AddNode(Vector2 position)
        {
            var newNode = new DialogueNode();
            newNode.id = System.Guid.NewGuid().ToString(); // Generate a unique ID for the new node

            if (dialogueTree != null)
            {
                dialogueTree.nodes.Add(newNode);
                var dialogueNodeEditor = new DialogueNodeEditor(position, 300, 100, nodeStyle, selectedNodeStyle, newNode); // Increased width to 300
                dialogueNodeEditor.SetOnRemoveNode(OnRemoveNode); // Subscribe to the remove event
                nodes.Add(dialogueNodeEditor);
            }
        }

        private void RemoveSelectedNode()
        {
            if (selectedNode != null)
            {
                if (selectedNode is DialogueNodeEditor nodeEditor)
                {
                    RemoveNodeConnections(nodeEditor);
                    dialogueTree.nodes.Remove(nodeEditor.dataNode);
                    nodes.Remove(selectedNode);
                    selectedNode = null;
                }
            }
        }

        private void OnRemoveNode(BaseNode node)
        {
            if (node is DialogueNodeEditor nodeEditor)
            {
                RemoveNodeConnections(nodeEditor);
                dialogueTree.nodes.Remove(nodeEditor.dataNode);
            }
            nodes.Remove(node);
        }

        private void RemoveNodeConnections(DialogueNodeEditor nodeEditor)
        {
            // Remove all connections associated with this node
            connections.RemoveAll(connection =>
                connection.outNode == nodeEditor || connection.inNode == nodeEditor);

            // Clear nextNodeId for options pointing to this node
            foreach (var node in nodes)
            {
                if (node is DialogueNodeEditor editor)
                {
                    foreach (var option in editor.dataNode.options)
                    {
                        if (option.nextNodeId == nodeEditor.dataNode.id)
                        {
                            option.nextNodeId = null;
                        }
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
            }
        }

        private void LoadDialogueTree()
        {
            var path = EditorUtility.OpenFilePanel("Select Dialogue Tree", "Assets", "asset");
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                dialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
                if (dialogueTree != null)
                {
                    LoadNodes();
                }
            }
        }

        private void LoadNodes()
        {
            nodes.Clear();
            connections.Clear(); // Clear existing connections
            if (dialogueTree != null)
            {
                foreach (var dataNode in dialogueTree.nodes)
                {
                    var dialogueNodeEditor = new DialogueNodeEditor(Vector2.zero, 300, 100, nodeStyle, selectedNodeStyle, dataNode); // Increased width to 300
                    dialogueNodeEditor.SetOnRemoveNode(OnRemoveNode); // Subscribe to the remove event
                    nodes.Add(dialogueNodeEditor);
                }

                // Recreate connections
                foreach (var node in nodes)
                {
                    if (node is DialogueNodeEditor nodeEditor)
                    {
                        foreach (var option in nodeEditor.dataNode.options)
                        {
                            if (!string.IsNullOrEmpty(option.nextNodeId))
                            {
                                var targetNode = nodes.Find(n => n is DialogueNodeEditor de && de.dataNode.id == option.nextNodeId) as DialogueNodeEditor;
                                if (targetNode != null)
                                {
                                    CreateConnection(nodeEditor, targetNode, nodeEditor.dataNode.options.IndexOf(option));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawNodes()
        {
            foreach (var node in nodes)
            {
                node.Draw();
            }
        }

        private void DrawConnections()
        {
            foreach (var connection in connections)
            {
                connection.Draw();
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                var guiChanged = nodes[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        OnLeftMouseDown(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && selectedOutNodeEditor != null)
                    {
                        DragConnection(e.mousePosition);
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (e.button == 0 && selectedOutNodeEditor != null)
                    {
                        var targetNode = nodes.Find(n => n.rect.Contains(e.mousePosition)) as DialogueNodeEditor;
                        if (targetNode != null && selectedOutNodeEditor != targetNode)
                        {
                            CreateConnection(selectedOutNodeEditor, targetNode, selectedOutOptionIndex);
                        }
                        ClearConnectionSelection();
                    }
                    break;
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
            if (selectedOutNodeEditor != null)
            {
                var startPoint = selectedOutNodeEditor.GetOptionRectCenterRight(selectedOutOptionIndex);
                Handles.DrawBezier(
                    startPoint,
                    mousePosition,
                    startPoint + Vector2.right * 50f,
                    mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );
            }
            GUI.changed = true;
        }

        private void OnLeftMouseDown(Vector2 mousePosition)
        {
            foreach (var node in nodes)
            {
                if (node.rect.Contains(mousePosition))
                {
                    selectedNode = node;
                    break;
                }
            }
        }

        public void SetOptionTargetNode(DialogueNodeEditor nodeEditor, int optionIndex)
        {
            selectedOutNodeEditor = nodeEditor;
            selectedOutOptionIndex = optionIndex;
        }
    }
}
