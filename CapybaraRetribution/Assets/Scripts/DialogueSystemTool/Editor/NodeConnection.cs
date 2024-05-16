using DialogueSystemTool.Data;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Editor
{
    public class NodeConnection
    {
        public BaseNode outNode; // The starting node of the connection
        public BaseNode inNode;  // The ending node of the connection

        /// <summary>
        /// Constructor for NodeConnection.
        /// </summary>
        /// <param name="outNode">The starting node of the connection.</param>
        /// <param name="inNode">The ending node of the connection.</param>
        public NodeConnection(BaseNode outNode, BaseNode inNode)
        {
            this.outNode = outNode;
            this.inNode = inNode;
        }

        /// <summary>
        /// Draws the connection between nodes.
        /// </summary>
        public void Draw()
        {
            Handles.DrawBezier(
                outNode.rect.center,
                inNode.rect.center,
                outNode.rect.center + Vector2.left * 50f,
                inNode.rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
        }
    }
}