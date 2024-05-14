using DialogueSystemTool.Data;
using UnityEditor;
using UnityEngine;

namespace DialogueSystemTool.Editor
{
    [CustomPropertyDrawer(typeof(DialogueOption))]
    public class DialogueOptionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var optionTextProp = property.FindPropertyRelative("optionText");
            var nextNodeIdProp = property.FindPropertyRelative("nextNodeId");
            var conditionsProp = property.FindPropertyRelative("conditions");

            // Draw fields
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, optionTextProp, new GUIContent("Option Text"));

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, nextNodeIdProp, new GUIContent("Next Node ID"));

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, conditionsProp, new GUIContent("Conditions"), true);

            EditorGUI.EndProperty();
        }

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