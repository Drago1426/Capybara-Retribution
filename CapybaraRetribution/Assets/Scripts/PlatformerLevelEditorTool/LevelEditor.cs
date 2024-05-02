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
            GetWindow<LevelEditor>("Level Editor");
        }

        private string[] items = new string[] { "Platform", "Trap", "Enemy", "Tile" };
        private int selectedItemIndex = 0;
        private Sprite selectedSprite;
        private Tilemap tilemap;
        private Tile selectedTile;

        // Customization variables
        private Texture2D backgroundTexture;
        private GameObject decorativePrefab;

        private void OnEnable()
        {
            tilemap = FindObjectOfType<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogWarning("No Tilemap found in the scene.");
            }
        }

        private void OnGUI()
        {
            GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter };
            GUILayout.Label("Platformer Level Editor Tool", headerStyle);

            GUILayout.Space(10); // Adds a bit of spacing after the header
            
            GUILayout.Label("Select an item to place", EditorStyles.boldLabel);
            selectedItemIndex = EditorGUILayout.Popup("Item Type", selectedItemIndex, items);

            if (selectedItemIndex == 3) // Tiles
            {
                selectedTile = EditorGUILayout.ObjectField("Select Tile", selectedTile, typeof(Tile), false) as Tile;
                TilemapDrawTool.ActiveTile = selectedTile; // Update the active tile for the draw tool

                if (GUILayout.Button("Activate Draw Tool"))
                {
                    ToolManager.SetActiveTool<TilemapDrawTool>();
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
                if (newObject.CompareTag("Platform"))
                {
                    newObject.AddComponent<BoxCollider2D>();
                }

                Debug.Log("Added: " + newObject.name + " with tag: " + newObject.tag);
            }
        }
    }
}