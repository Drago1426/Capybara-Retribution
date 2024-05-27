using System;
using System.IO;
using UnityEngine;

    public class CharacterLoader : MonoBehaviour
    {
        public SpriteRenderer furRenderer;
        public SpriteRenderer eyesRenderer;
        public SpriteRenderer hatRenderer;
        public SpriteRenderer weaponRenderer;

        public FurCollection furOptions;
        public EyeCollection eyesOptions;
        public HatCollection hatOptions;
        public WeaponCollection weaponOptions;

        private void Start()
        {
            LoadCharacter();
        }

        private void LoadCharacter()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, "characterData.json");
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                CharacterData characterData = JsonUtility.FromJson<CharacterData>(json);

                ApplyCharacter(characterData);
            }
        }
        
        private void ApplyCharacter(CharacterData characterData)
        {
            // Apply the saved assets
            Fur selectedFur = furOptions.items[characterData.furIndex];
            furRenderer.color = selectedFur.furColor;
            // Ensure you have a default sprite for the furRenderer, which is not changing per fur option

            eyesRenderer.sprite = eyesOptions.items[characterData.eyesIndex].eyesSprite;
            hatRenderer.sprite = hatOptions.items[characterData.hatIndex].hatSprite;
            weaponRenderer.sprite = weaponOptions.items[characterData.weaponIndex].weaponSprite;
        }
    }
