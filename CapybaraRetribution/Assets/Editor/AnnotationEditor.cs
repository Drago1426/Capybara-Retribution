using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnnotationManager))]
public class AnnotationEditor : Editor
{
    // The chosen annotation visually
    private Annotation selected;
    
    private void OnSceneGUI()
    {
        // Converts the target object to usable data
        var data = (AnnotationManager)target;

        DrawGUI(data);
        
        // Here we'll draw all the annotations
        foreach (var annotation in data.annotaions)
        {
            // Change the base colour to white
            Handles.color = Color.white;
            
            // Draw the string
            var pos = annotation.position + Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up) * AnnotationManager.AnnotationOffset;
            Handles.Label(pos, annotation.text);

            // This will change the button color
            Handles.color = new Color(1, 1, 1, 0.5f);
            if (selected == annotation)
            {
                Handles.color = Color.yellow;
            }
            
            if (Handles.Button(annotation.position, Quaternion.identity, AnnotationManager.CapSize, AnnotationManager.CapSize, Handles.DotHandleCap))
            {
                // When we press on the icon on the scene, we'll set the selected annotation
                selected = annotation;
            }

            // If this is the chosen/selected item, we'll show the position changer
            if (selected == annotation)
            {
                annotation.position = Handles.PositionHandle(annotation.position, Quaternion.identity);
            }
        }
    }

    private void DrawGUI(AnnotationManager data)
    {
        // Control variables
        const float margin = 4;
        var controlRect = new Rect(20, 40, 180, 20);
        
        Handles.BeginGUI();
        GUI.Box(new Rect(10, 10, 200, 110), "Annotation Editor");

        // If we have a selected item , we'll show its controls
        if (selected != null)
        {
            selected.text = GUI.TextField(controlRect, selected.text);
            controlRect.y += controlRect.height + margin;

            if (GUI.Button(controlRect, "Delete"))
            {
                data.annotaions.Remove(selected);
                selected = null;
            }
            controlRect.y += controlRect.height + margin;
        }
        
        // Create and check if the button is pressed
        if (GUI.Button(controlRect, "Add Annotation"))
        {
            var a = new Annotation();
            data.annotaions.Add(a);
            selected = a;
        }
        
        Handles.EndGUI();
    }
}
