using UnityEditor;
using UnityEngine;
using DialogueSystemTool.Data;

namespace DialogueSystemTool.Editor
{
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var variableNameProp = property.FindPropertyRelative("variableName");
            var expectedValueProp = property.FindPropertyRelative("expectedValue");

            // Draw fields
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, variableNameProp, new GUIContent("Variable Name"));

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, expectedValueProp, new GUIContent("Expected Value"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}