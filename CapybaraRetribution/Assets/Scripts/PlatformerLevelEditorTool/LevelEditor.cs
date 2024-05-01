namespace PlatformerLevelEditor
{
    using UnityEditor;
    using UnityEngine;

    public class LevelEditor : EditorWindow
    {
        
        [MenuItem("Window/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditor>("Level Editor");
        }

        private string[] items = new string[] { "Platform", "Trap", "Enemy" };
        private int selectedItemIndex = 0;
        private Sprite selectedSprite;

        // Customization variables
        private Texture2D backgroundTexture;
        private Color platformColor = Color.white;
        private GameObject decorativePrefab;
        private Vector2 backgroundSize = new Vector2(10, 5); // Default size of the background

        private void OnGUI()
        {
            GUILayout.Label("Select an item to place", EditorStyles.boldLabel);
            
            // Item placement section
            selectedItemIndex = EditorGUILayout.Popup("Item Type", selectedItemIndex, items);
            selectedSprite = EditorGUILayout.ObjectField("Select Sprite", selectedSprite, typeof(Sprite), false) as Sprite;
            if (GUILayout.Button("Add Item"))
            {
                CreateGameObjectWithSprite();
            }

            // Divider
            GUILayout.Space(20);
            GUILayout.Label("Level Customization", EditorStyles.boldLabel);

            // Level Customizer section
            backgroundTexture = EditorGUILayout.ObjectField("Background", backgroundTexture, typeof(Texture2D), false) as Texture2D;
            
            // Background size input fields
            backgroundSize = EditorGUILayout.Vector2Field("Background Size", backgroundSize);

            // Button to create a background with specific size
            if (GUILayout.Button("Create Background"))
            {
                CreateBackground();
            }
        }

        void CreateGameObjectWithSprite()
        {
            if (selectedSprite != null)
            {
                GameObject newObject = new GameObject(selectedSprite.name);
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                renderer.sprite = selectedSprite;
                newObject.tag = items[selectedItemIndex];

                // Add a BoxCollider2D if it's a platform
                if (newObject.tag == "Platform")
                {
                    newObject.AddComponent<BoxCollider2D>();
                }

                Debug.Log("Added: " + newObject.name + " with tag: " + newObject.tag);
            }
        }

        void CreateBackground()
        {
            if (backgroundTexture)
            {
                GameObject backgroundObject = GameObject.Find("Background") ?? new GameObject("Background");
                SpriteRenderer renderer = backgroundObject.GetComponent<SpriteRenderer>() ?? backgroundObject.AddComponent<SpriteRenderer>();
                renderer.sprite = Sprite.Create(backgroundTexture, new Rect(0.0f, 0.0f, backgroundTexture.width, backgroundTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                renderer.size = backgroundSize; // Set the size of the sprite
                renderer.sortingLayerName = "Background";
                backgroundObject.transform.localScale = Vector3.one; // Set scale to 1 for consistency
                Debug.Log("Background created with size: " + backgroundSize);
            }
        }
    }
}

