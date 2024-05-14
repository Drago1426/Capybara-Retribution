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
        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private BaseNode selectedNode;
        private DialogueTree dialogueTree;

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
            selectedNodeStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            nodes = new List<BaseNode>();
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
                    foreach (var dataNode in dialogueTree.nodes)
                    {
                        nodes.Add(
                            new DialogueNodeEditor(Vector2.zero, 200, 100, nodeStyle, selectedNodeStyle, dataNode));
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Save Dialogue Tree"))
            {
                SaveDialogueTree();
            }

            if (GUILayout.Button("Load Dialogue Tree"))
            {
                LoadDialogueTree();
            }

            DrawNodes();
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
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
            DialogueNode dataNode = new DialogueNode(); // Creates new data node
            nodes.Add(new DialogueNodeEditor(mousePosition, 200, 100, nodeStyle, selectedNodeStyle, dataNode));
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
    }
}