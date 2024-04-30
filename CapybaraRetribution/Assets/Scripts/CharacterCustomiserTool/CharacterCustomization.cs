using UnityEngine;
using UnityEngine.UI;

    public class CharacterCustomization : MonoBehaviour
    {
        public Transform accessoryAttachmentPoint;

        public SpriteRenderer furSpriteRenderer;
        public SpriteRenderer eyeSpriteRenderer;
        
        public Slider furColorSlider;
        public Slider eyeColorSlider;
        
        public void ChangeFurColor(FurColor furColor)
        {
            float r = furColorSlider.value * 2f; 
            float g = Mathf.Abs(furColorSlider.value - 0.5f) * 2f; 
            float b = Mathf.Abs(furColorSlider.value - 1f) * 2f; 
            
            Color newFurColor = new Color(r, g, b);
            furSpriteRenderer.color = newFurColor;        }

        public void ChangeEyeColor(EyeColor eyeColor)
        {
            float r = Mathf.Abs(eyeColorSlider.value - 0.5f) * 2f;  
            float g = eyeColorSlider.value * 2f; 
            float b = Mathf.Abs(eyeColorSlider.value - 1f) * 2f; 

            Color newEyeColor = new Color(r, g, b);
            eyeSpriteRenderer.color = newEyeColor;
        }

        public void AddAccessory(GameObject accessoryPrefab)
        {
            GameObject accessoryInstance = Instantiate(accessoryPrefab, accessoryAttachmentPoint);
        }

        public void RemoveAccessory(GameObject accessoryInstance)
        {
            Destroy(accessoryInstance);
        }
    }
