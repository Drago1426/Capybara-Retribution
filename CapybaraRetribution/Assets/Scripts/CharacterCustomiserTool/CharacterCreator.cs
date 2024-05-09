using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCreator : MonoBehaviour
{
    // Renderers 
    public SpriteRenderer furRenderer;
    public SpriteRenderer eyesRenderer;
    public SpriteRenderer hatRenderer;
    public SpriteRenderer weaponRenderer;

    //  Options
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
     private void Start()
    {
        UpdateCharacter();
    }

    public void SelectFur(int index)
    {
        furIndex = index;
        UpdateCharacter();
    }

    public void SelectEyes(int index)
    {
        eyesIndex = index;
        UpdateCharacter();
    }

    public void SelectHat(int index)
    {
        hatIndex = index;
        UpdateCharacter();
    }

    public void SelectWeapon(int index)
    {
        weaponIndex = index;
        UpdateCharacter();
    }

    public void NextFur()
    {
        furIndex = (furIndex + 1) % furOptions.Length;
        UpdateCharacter();
    }

    public void PrevFur()
    {
        furIndex = (furIndex - 1 + furOptions.Length) % furOptions.Length;
        UpdateCharacter();
    }

    public void NextEyes()
    {
        eyesIndex = (eyesIndex + 1) % eyesOptions.Length;
        UpdateCharacter();
    }

    public void PrevEyes()
    {
        eyesIndex = (eyesIndex - 1 + eyesOptions.Length) % eyesOptions.Length;
        UpdateCharacter();
    }

    public void NextHat()
    {
        hatIndex = (hatIndex + 1) % hatOptions.Length;
        UpdateCharacter();
    }

    public void PrevHat()
    {
        hatIndex = (hatIndex - 1 + hatOptions.Length) % hatOptions.Length;
        UpdateCharacter();
    }

    public void NextWeapon()
    {
        weaponIndex = (weaponIndex + 1) % weaponOptions.Length;
        UpdateCharacter();
    }

    public void PrevWeapon()
    {
        weaponIndex = (weaponIndex - 1 + weaponOptions.Length) % weaponOptions.Length;
        UpdateCharacter();
    }

    internal void UpdateCharacter()
    {
        
        // Currently Selected Option
        selectedFur = furOptions.items[furIndex];
        selectedEyes = eyesOptions.items[eyesIndex];
        selectedHat = hatOptions.items[hatIndex];
        selectedWeapon = weaponOptions.items[weaponIndex];

        // Currently Selected Asset
        furRenderer.sprite = selectedFur.furSprite;
        eyesRenderer.sprite = selectedEyes.eyesSprite;
        hatRenderer.sprite = selectedHat.hatSprite;
        weaponRenderer.sprite = selectedWeapon.weaponSprite;
        
        // Currently Selected Text
        furNameText.text = selectedFur.furName;
        eyesNameText.text = selectedEyes.eyesName;
        hatNameText.text = selectedHat.hatName;
        weaponNameText.text = selectedWeapon.weaponName;
    }

    public void UpdateCharacter(ScriptableObject eyeCollection, ScriptableObject furCollection, ScriptableObject hatCollection, ScriptableObject weaponCollection)
    {
        throw new System.NotImplementedException();
    }
}