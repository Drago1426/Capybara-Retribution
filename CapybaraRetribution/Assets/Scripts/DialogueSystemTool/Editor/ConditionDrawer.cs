using UnityEditor;
using UnityEngine;
using DialogueSystemTool.Data;

namespace DialogueSystemTool.Editor
{
    /// <summary>
    /// Custom property drawer for the Condition class.
    /// </summary>
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Override to specify how the Condition property should be drawn in the Inspector.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var variableNameProp = property.FindPropertyRelative("variableName");
            var expectedValueProp = property.FindPropertyRelative("expectedValue");

            // Draw the variable name field
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, variableNameProp, new GUIContent("Variable Name"));

            // Draw the expected value field
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, expectedValueProp, new GUIContent("Expected Value"));

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Override to specify the height of the Condition property in the Inspector.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}