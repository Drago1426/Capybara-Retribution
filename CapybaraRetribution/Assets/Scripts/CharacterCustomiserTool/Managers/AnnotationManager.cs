using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    // This class will only be used inside the Editor
    public class AnnotationManager : MonoBehaviour
    {
        public const float CapSize = 0.15f;
        public const float CapMargin = 0.2f;
        public static readonly Vector3 AnnotationOffset = Vector3.right * (CapSize + CapMargin);
        
        // The annotations in the scene
        public List<Annotation> annotaions;

        // When the script resets (incl. being added to a game object) this function will run
        private void Reset()
        {
            gameObject.tag = "EditorOnly";
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject != gameObject)
            {
                var gizmoColor = UnityEditor.Handles.color;
                UnityEditor.Handles.color = new Color(1, 1, 1, 0.75f);

                foreach (var annotation in annotaions)
                {
                    // Draw the string
                    var pos = annotation.position + Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up) * AnnotationManager.AnnotationOffset;
                    UnityEditor.Handles.Label(pos, annotation.text);
   
                }
                
                UnityEditor.Handles.color = gizmoColor;
            }
        }
#endif
    }
}