using UnityEngine;
using UnityEditor;

public class CharacterCustomizerWindow : EditorWindow
{
    private SerializedObject furColorObject;
    private SerializedObject eyeColorObject;
    private SerializedObject accessoryObject;
    private SerializedObject weaponObject;

    [MenuItem("Character Customizer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<CharacterCustomizerWindow>("Character Customizer");
    }
}
