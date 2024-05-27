using System.IO;
using UnityEngine;
using UnityEditor;
using TMPro;

public class CharacterCreator : MonoBehaviour
{
    // Renderers 
    public SpriteRenderer furRenderer;
    public SpriteRenderer eyesRenderer;
    public SpriteRenderer hatRenderer;
    public SpriteRenderer weaponRenderer;

    // Options
    public FurCollection furOptions;
    public EyeCollection eyesOptions;
    public HatCollection hatOptions;
    public WeaponCollection weaponOptions;

    // Text 
    public TextMeshProUGUI furNameText;
    public TextMeshProUGUI eyesNameText;
    public TextMeshProUGUI hatNameText;
    public TextMeshProUGUI weaponNameText;

    // Current Selected 
    public Fur selectedFur;
    public Eyes selectedEyes;
    public Hat selectedHat;
    public Weapon selectedWeapon;

    private int furIndex = 0;
    private int eyesIndex = 0;
    private int hatIndex = 0;
    private int weaponIndex = 0;

    private const string saveFilePath = "characterData.json";
    private void Start()
    {
        LoadCharacter();
        UpdateCharacter();
    }

    public void NextFur()
    {
        furIndex = (furIndex + 1) % furOptions.items.Count;
        UpdateCharacter();
    }

    public void PrevFur()
    {
        furIndex = (furIndex - 1 + furOptions.items.Count) % furOptions.items.Count;
        UpdateCharacter();
    }

    public void NextEyes()
    {
        eyesIndex = (eyesIndex + 1) % eyesOptions.items.Count;
        UpdateCharacter();
    }

    public void PrevEyes()
    {
        eyesIndex = (eyesIndex - 1 + eyesOptions.items.Count) % eyesOptions.items.Count;
        UpdateCharacter();
    }

    public void NextHat()
    {
        hatIndex = (hatIndex + 1) % hatOptions.items.Count;
        UpdateCharacter();
    }

    public void PrevHat()
    {
        hatIndex = (hatIndex - 1 + hatOptions.items.Count) % hatOptions.items.Count;
        UpdateCharacter();
    }

    public void NextWeapon()
    {
        weaponIndex = (weaponIndex + 1) % weaponOptions.items.Count;
        UpdateCharacter();
    }

    public void PrevWeapon()
    {
        weaponIndex = (weaponIndex - 1 + weaponOptions.items.Count) % weaponOptions.items.Count;
        UpdateCharacter();
    }

    internal void UpdateCharacter()
    {
        // Check for missing assets before updating
        if (CheckForMissingAssets())
        {
            return;
        }

        // Currently Selected Option
        selectedFur = furOptions.items[furIndex];
        selectedEyes = eyesOptions.items[eyesIndex];
        selectedHat = hatOptions.items[hatIndex];
        selectedWeapon = weaponOptions.items[weaponIndex];

        // Currently Selected Asset
        furRenderer.color = selectedFur.furColor;
        eyesRenderer.sprite = selectedEyes.eyesSprite;
        hatRenderer.sprite = selectedHat.hatSprite;
        weaponRenderer.sprite = selectedWeapon.weaponSprite;

        // Currently Selected Text
        furNameText.text = selectedFur.furName;
        eyesNameText.text = selectedEyes.eyesName;
        hatNameText.text = selectedHat.hatName;
        weaponNameText.text = selectedWeapon.weaponName;
    }

    private bool CheckForMissingAssets()
    {
        if (furOptions == null || furOptions.items.Count == 0)
        {
            EditorUtility.DisplayDialog("Missing Asset", "Fur options are missing!", "OK");
            return true;
        }
        if (eyesOptions == null || eyesOptions.items.Count == 0)
        {
            EditorUtility.DisplayDialog("Missing Asset", "Eyes options are missing!", "OK");
            return true;
        }
        if (hatOptions == null || hatOptions.items.Count == 0)
        {
            EditorUtility.DisplayDialog("Missing Asset", "Hat options are missing!", "OK");
            return true;
        }
        if (weaponOptions == null || weaponOptions.items.Count == 0)
        {
            EditorUtility.DisplayDialog("Missing Asset", "Weapon options are missing!", "OK");
            return true;
        }
        return false;
    }

    public void SaveCharacter()
    {
        CharacterData characterData = new CharacterData()
        {
            furIndex = furIndex,
            eyesIndex = eyesIndex,
            hatIndex = hatIndex,
            weaponIndex = weaponIndex
        };
        string json = JsonUtility.ToJson(characterData);
        File.WriteAllText(Path.Combine(Application.persistentDataPath,saveFilePath),json);
    }

    public void LoadCharacter()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveFilePath);
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            CharacterData characterData = JsonUtility.FromJson<CharacterData>(json);
            
            furIndex = characterData.furIndex;
            eyesIndex = characterData.eyesIndex;
            hatIndex = characterData.hatIndex;
            weaponIndex = characterData.weaponIndex;

            UpdateCharacter();
        }
    }

    public void UpdateCharacter(ScriptableObject eyeCollection, ScriptableObject furCollection, ScriptableObject hatCollection, ScriptableObject weaponCollection)
    {
        throw new System.NotImplementedException();
    }
}
