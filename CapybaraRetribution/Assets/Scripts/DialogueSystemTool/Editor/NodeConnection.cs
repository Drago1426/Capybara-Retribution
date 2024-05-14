using DialogueSystemTool.Data;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Editor
{
    public class NodeConnection
    {
        public BaseNode startNode;
        public BaseNode endNode;

        public NodeConnection(BaseNode startNode, BaseNode endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                startNode.rect.center,
                endNode.rect.center,
                startNode.rect.center + Vector2.left * 50f,
                endNode.rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
        }
    }
}