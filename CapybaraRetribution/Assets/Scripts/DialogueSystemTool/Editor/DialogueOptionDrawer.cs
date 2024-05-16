using DialogueSystemTool.Data;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Editor
{
    /// <summary>
    /// Custom property drawer for the DialogueOption class.
    /// </summary>
    [CustomPropertyDrawer(typeof(DialogueOption))]
    public class DialogueOptionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Override to specify how the DialogueOption property should be drawn in the Inspector.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var optionTextProp = property.FindPropertyRelative("optionText");
            var nextNodeIdProp = property.FindPropertyRelative("nextNodeId");
            var conditionsProp = property.FindPropertyRelative("conditions");

            // Draw the option text field
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, optionTextProp, new GUIContent("Option Text"));

            // Draw the next node ID field
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, nextNodeIdProp, new GUIContent("Next Node ID"));

            // Draw the conditions field
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, conditionsProp, new GUIContent("Conditions"), true);

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Override to specify the height of the DialogueOption property in the Inspector.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
            var conditionsProp = property.FindPropertyRelative("conditions");
            if (conditionsProp.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(conditionsProp);
            }
            return height;
        }
    }
}
