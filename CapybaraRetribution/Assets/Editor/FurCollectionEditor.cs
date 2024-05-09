using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FurCollection))]
public class FurCollectionEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // Create the root
        var root = new VisualElement { name = "furCollectionRoot" };
        var headerLabel = new Label
        {
            name = "HeaderLabel",
            text = "Fur Collection Settings"
        };
        
        
        
        root.Add(headerLabel);
        return root;
    }
}