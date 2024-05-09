using System;
using UnityEngine;
using UnityEditor;

public class CharacterCreatorWindow : EditorWindow
{
    private CharacterCreator characterCreator;

    [MenuItem("Window/Character Creator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterCreatorWindow>("Character Creator");
    }

    private void CreateGUI()
    {
//        throw new NotImplementedException();
    }

    // void OnGUI()
    // {
    //     characterCreator = FindObjectOfType<CharacterCreator>();
    //
    //     if (characterCreator == null)
    //     {
    //         EditorGUILayout.LabelField("Please add CharacterCreator script to a GameObject in the scene.");
    //         return;
    //     }
    //
    //     GUILayout.Label("Select Fur:");
    //     characterCreator.selectedFur = (Fur)EditorGUILayout.ObjectField(characterCreator.selectedFur, typeof(Fur), false);
    //
    //     GUILayout.Label("Select Eyes:");
    //     characterCreator.selectedEyes = (Eyes)EditorGUILayout.ObjectField(characterCreator.selectedEyes, typeof(Eyes), false);
    //
    //     GUILayout.Label("Select Hat:");
    //     characterCreator.selectedHat = (Hat)EditorGUILayout.ObjectField(characterCreator.selectedHat, typeof(Hat), false);
    //
    //     GUILayout.Label("Select Weapon:");
    //     characterCreator.selectedWeapon = (Weapon)EditorGUILayout.ObjectField(characterCreator.selectedWeapon, typeof(Weapon), false);
    //
    //     if (GUILayout.Button("Apply Changes"))
    //     {
    //         characterCreator.UpdateCharacter();
    //     }
    //
    //     if (GUILayout.Button("Reset"))
    //     {
    //         ResetCharacter();
    //     }
    // }

    private void ResetCharacter()
    {
        // Reset all selected assets to null
        characterCreator.selectedFur = null;
        characterCreator.selectedEyes = null;
        characterCreator.selectedHat = null;
        characterCreator.selectedWeapon = null;
        characterCreator.UpdateCharacter();
    }
}