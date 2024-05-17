namespace PlatformerLevelEditorTool
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEditor.EditorTools;

    // Define a custom editor tool for drawing on the tilemap
    [EditorTool("Tilemap Draw Tool", typeof(Tilemap))]
    public class TilemapDrawTool : EditorTool
    {
        private Tilemap tilemap;
        private Tile selectedTile;

        // Store the reference to the selected tile from the LevelEditor
        public static Tile ActiveTile { get; set; }

        public override void OnActivated()
        {
            tilemap = target as Tilemap;
        }

        public override void OnToolGUI(EditorWindow window)
        {
            selectedTile = ActiveTile;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event e = Event.current;

            if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0) // Left click to place tile
            {
                Vector3 point = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                Vector3Int cellPosition = tilemap.WorldToCell(point);
                tilemap.SetTile(cellPosition, selectedTile);
                e.Use();
            }
            if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 1) // Right click to remove tile
            {
                Vector3 point = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                Vector3Int cellPosition = tilemap.WorldToCell(point);
                tilemap.SetTile(cellPosition, null);
                e.Use();
            }
        }
    }
    

    public class LevelEditor : EditorWindow
    {
        [MenuItem("Window/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditor>("Platformer Level Editor Tool");
        }

        private string[] items = new string[] { "Trap", "Enemy", "Tile", "Player"};
        private int selectedItemIndex = 0;
        private Sprite selectedSprite;
        private Tilemap tilemap;
        private Tile selectedTile;

        // Background settings
        private string[] backgroundOptions = new string[] { "Morning", "Noon", "Night" };
        private int selectedBackgroundIndex = 0; // Default to 'Morning'
        private Sprite[] backgrounds = new Sprite[3];
        private GameObject currentBackground;
        

        private void OnEnable()
        {
            tilemap = FindObjectOfType<Tilemap>();
            LoadBackgrounds();
            if (tilemap == null)
            {
                Debug.LogWarning("No Tilemap found in the scene.");
            }
        }
        
        private void LoadBackgrounds()
        {
            backgrounds[0] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Backgrounds/Morning.png");
            backgrounds[1] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Backgrounds/Noon.png");
            backgrounds[2] = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Backgrounds/Night.png");
        }


        private void OnGUI()
        {
            GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter };
            GUILayout.Label("Platformer Level Editor Tool", headerStyle);

            GUILayout.Space(10);
            
            GUILayout.Label("Select an item to place", EditorStyles.boldLabel);
            selectedItemIndex = EditorGUILayout.Popup("Item Type", selectedItemIndex, items);

            if (selectedItemIndex == 2)
            {
                selectedTile = EditorGUILayout.ObjectField("Select Tile", selectedTile, typeof(Tile), false) as Tile;
                TilemapDrawTool.ActiveTile = selectedTile;

                if (GUILayout.Button("Activate Draw Tool"))
                {
                    if (selectedTile == null)
                    {
                        EditorUtility.DisplayDialog(
                            "No Tile Selected",
                            "Please select a tile before activating the draw tool.",
                            "OK"
                        );
                        return;
                    }
                    
                    if (tilemap == null)
                    {
                        tilemap = FindObjectOfType<Tilemap>();
                        
                        if (tilemap == null)
                        {
                            EditorUtility.DisplayDialog(
                                "No Tilemap Found",
                                "There is no Tilemap in the scene. Please add a Tilemap before activating the draw tool.",
                                "OK"
                            );
                            return;
                        }
                    }
                    
                    Selection.activeGameObject = tilemap.gameObject;
                    ToolManager.SetActiveTool<TilemapDrawTool>();
                }
            }
            else if (selectedItemIndex == 3) // Player
            {
                selectedSprite = EditorGUILayout.ObjectField("Select Player Sprite", selectedSprite, typeof(Sprite), false) as Sprite;
                if (GUILayout.Button("Place Player"))
                {
                    PlacePlayer();
                }
            }
            else
            {
                selectedSprite = EditorGUILayout.ObjectField("Select Sprite", selectedSprite, typeof(Sprite), false) as Sprite;
                if (GUILayout.Button("Add Item"))
                {
                    CreateGameObjectWithSprite();
                }
            }
            
            GUILayout.Space(10);
            GUILayout.Label("Background Settings", EditorStyles.boldLabel);

            // Dropdown for selecting the background
            selectedBackgroundIndex = EditorGUILayout.Popup("Select Background", selectedBackgroundIndex, backgroundOptions);

            // Button to apply the selected background
            if (GUILayout.Button("Apply Background"))
            {
                ApplyBackgroundChange();
            }
        }

        private void ApplyBackgroundChange()
        {
            if (currentBackground != null) DestroyImmediate(currentBackground);

            currentBackground = new GameObject("Dynamic Background");
            var renderer = currentBackground.AddComponent<SpriteRenderer>();
            renderer.sprite = backgrounds[selectedBackgroundIndex];
            renderer.sortingLayerName = "Background"; // Ensure it renders behind other objects
            renderer.sortingOrder = -10;

            // Attach the ParallaxBackground script to the new background GameObject
            currentBackground.AddComponent<ParallaxBackground>();
        }
        
        void CreateGameObjectWithSprite()
        {
            if (selectedSprite != null)
            {
                GameObject newObject = new GameObject(selectedSprite.name);
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                renderer.sprite = selectedSprite;
                newObject.tag = items[selectedItemIndex];

                // Add components based on the type of the object
                switch (newObject.tag)
                {
                    case "Trap":
                        SetupTrap(newObject);
                        break;
                    case "Enemy":
                        SetupEnemy(newObject);
                        break;
                }

                Debug.Log("Added: " + newObject.name + " with tag: " + newObject.tag);
            }
            else
            {
                EditorUtility.DisplayDialog("No Sprite Selected", "Please select a sprite.", "OK");
            }
        }

        void SetupTrap(GameObject trap)
        {
            BoxCollider2D collider = trap.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            trap.AddComponent<Rigidbody2D>().isKinematic = true;
            trap.AddComponent<TrapBehavior>();
        }

        void SetupEnemy(GameObject enemy)
        {
            BoxCollider2D collider = enemy.AddComponent<BoxCollider2D>();
            Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Commonly enemies shouldn't rotate in 2D platformers
            enemy.AddComponent<Animator>(); // Assuming there is an Animator component needed
            enemy.AddComponent<EnemyController>(); // Your custom enemy controller script
        }
        
        void PlacePlayer()
        {
            // Check if there is already a player in the scene
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                EditorUtility.DisplayDialog("Player Exists", "There is already a player in the game.", "OK");
                return;
            }

            if (selectedSprite == null)
            {
                EditorUtility.DisplayDialog("No Sprite Selected", "Please select a sprite for the player.", "OK");
                return;
            }

            GameObject playerObject = new GameObject("Player");
            playerObject.tag = "Player";

            SpriteRenderer renderer = playerObject.AddComponent<SpriteRenderer>();
            renderer.sprite = selectedSprite;

            playerObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rb = playerObject.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            playerObject.AddComponent<PlayerController>();
            AudioSource audioSource = playerObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            Debug.Log("Player placed in the game.");
        }
    }
}