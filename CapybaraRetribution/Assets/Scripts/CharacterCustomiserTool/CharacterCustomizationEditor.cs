using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterCustomization))]
public class CharacterCustomizationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterCustomization customization = (CharacterCustomization)target;

        GUILayout.Space(10);

        GUILayout.Label("Customization Options", EditorStyles.boldLabel);

        customization.furSpriteRenderer.material.color = EditorGUILayout.ColorField("Fur Color", customization.furSpriteRenderer.material.color);
        customization.eyeSpriteRenderer.material.color = EditorGUILayout.ColorField("Eye Color", customization.eyeSpriteRenderer.material.color);

        Accessories accessories = (Accessories)EditorGUILayout.ObjectField("Accessories", null, typeof(Accessories), false);

        if (accessories != null)
        {
            foreach (GameObject accessoryPrefab in accessories.accessoryPrefabs)
            {
                if (GUILayout.Button("Add " + accessoryPrefab.name))
                {
                    customization.AddAccessory(accessoryPrefab);
                }
            }
        }
    }
}