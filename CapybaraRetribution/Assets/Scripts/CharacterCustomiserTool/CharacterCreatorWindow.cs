using System;
using UnityEngine;
using UnityEditor;

public class CharacterCreatorWindow : EditorWindow
{
    private CharacterCreator characterCreator;

    // Paths to your asset collections
    private string eyeCollectionPath = "Assets/ScriptableObjects/Collections/Eye Collection.asset";
    private string furCollectionPath = "Assets/ScriptableObjects/Collections/Fur Collection.asset";
    private string hatCollectionPath = "Assets/ScriptableObjects/Collections/Hat Collection.asset";
    private string weaponCollectionPath = "Assets/ScriptableObjects/Collections/Weapon Collection.asset";

    // Loaded asset collections
    private ScriptableObject EyeCollection;
    private ScriptableObject FurCollection;
    private ScriptableObject HatCollection;
    private ScriptableObject WeaponCollection;

    [MenuItem("Window/Character Creator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterCreatorWindow>("Character Creator");
    }

    private void OnEnable()
    {
        // Load the asset collections when the window is enabled
        EyeCollection = AssetDatabase.LoadAssetAtPath<ScriptableObject>(eyeCollectionPath);
        FurCollection = AssetDatabase.LoadAssetAtPath<ScriptableObject>(furCollectionPath);
        HatCollection = AssetDatabase.LoadAssetAtPath<ScriptableObject>(hatCollectionPath);
        WeaponCollection = AssetDatabase.LoadAssetAtPath<ScriptableObject>(weaponCollectionPath);
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Creator", EditorStyles.boldLabel);

        if (GUILayout.Button("Reload Collections"))
        {
            OnEnable(); // Reload collections
        }

        if (EyeCollection == null || FurCollection == null || HatCollection == null || WeaponCollection == null)
        {
            EditorGUILayout.HelpBox("One or more collections could not be loaded. Please check the paths.", MessageType.Error);
            return;
        }

        // Manually draw inspector-like controls
        DrawCollectionInspector("Eye Collection", ref EyeCollection);
        DrawCollectionInspector("Fur Collection", ref FurCollection);
        DrawCollectionInspector("Hat Collection", ref HatCollection);
        DrawCollectionInspector("Weapon Collection", ref WeaponCollection);

        GUILayout.Space(10);

        if (GUILayout.Button("Create/Update Character"))
        {
            // Here you would invoke your character creator logic
            if (characterCreator != null)
            {
                characterCreator.UpdateCharacter(EyeCollection, FurCollection, HatCollection, WeaponCollection);
            }
        }
    }

    // A method to mimic OnInspectorGUI drawing for a collection
    void DrawCollectionInspector(string label, ref ScriptableObject collection)
    {
        GUILayout.Label(label + ":", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(collection, typeof(ScriptableObject), false);
    }
}
