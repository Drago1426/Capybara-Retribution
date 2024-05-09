using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(HatCollection))]
public class HatCollectionEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // Create the root visual element
        var root = new VisualElement();

        // Add a simple label to test visibility
        var testLabel = new Label("Hat Collection Settings");
        testLabel.style.fontSize = 14;
        testLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        testLabel.style.marginBottom = 10;
        root.Add(testLabel);

        // Retrieve the target HatCollection
        HatCollection hatCollection = (HatCollection)target;

        // Create a container for the ListView
        var listViewContainer = new VisualElement
        {
            style =
            {
                marginTop = 5,
                marginBottom = 5,
                backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.2f),
                borderLeftWidth = 2,
                borderRightWidth = 2,
                borderTopWidth = 2,
                borderBottomWidth = 2,
            }
        };

        // TextField for adding new Hat items
        var newItemNameField = new TextField("New Hat Name");
        newItemNameField.style.marginBottom = 8;
        root.Add(newItemNameField);

        // ObjectField for selecting a sprite for the new Hat
        var newHatSpriteField = new ObjectField("New Hat Sprite");
        newHatSpriteField.objectType = typeof(Sprite);
        newHatSpriteField.style.marginBottom = 8;
        root.Add(newHatSpriteField);

        // Add button for new Hat items
        var addButton = new Button(() =>
        {
            // Create and add new Hat item
            if (!string.IsNullOrWhiteSpace(newItemNameField.value))
            {
                // Ensure a path is set for saving the new Hat ScriptableObject
                string path = EditorUtility.SaveFilePanelInProject("Save New Hat", newItemNameField.value, "asset", "Please enter a file name to save the new hat item.");
                if (string.IsNullOrEmpty(path))
                    return;

                Hat newHat = CreateInstance<Hat>();
                newHat.hatName = newItemNameField.value;
                newHat.hatSprite = (Sprite)newHatSpriteField.value;

                // Save the new Hat as a ScriptableObject in the selected path
                AssetDatabase.CreateAsset(newHat, path);
                AssetDatabase.SaveAssets();

                hatCollection.items.Add(newHat);
                hatCollection.bodyParts = hatCollection.items; // Synchronize the lists
                newItemNameField.value = "";
                newHatSpriteField.value = null;

                // Refresh the ListView
                listViewContainer.Clear();
                PopulateListView(listViewContainer, hatCollection.bodyParts);
                EditorUtility.SetDirty(target); // Mark the asset as modified
            }
        })
        {
            text = "Add New Hat"
        };
        root.Add(addButton);

        // List to keep track of selected items
        List<Hat> selectedItems = new List<Hat>();

        // ListView setup for Hat items
        ListView listView = new ListView(hatCollection.bodyParts, 20, () => new Label(), (element, index) =>
        {
            (element as Label).text = hatCollection.bodyParts[index].hatName;
            // Make the label clickable and navigate to the ScriptableObject when clicked
            element.RegisterCallback<ClickEvent>(evt =>
            {
                Selection.activeObject = hatCollection.bodyParts[index];
                EditorGUIUtility.PingObject(hatCollection.bodyParts[index]);
            });
        });
        listView.selectionType = SelectionType.Multiple;
        listView.onSelectionChange += selected =>
        {
            selectedItems.Clear();
            foreach (var item in selected)
                selectedItems.Add((Hat)item);
        };
        listView.style.flexGrow = 1.0f;
        listViewContainer.Add(listView);

        // Delete button to remove selected Hat items
        var deleteButton = new Button(() =>
        {
            foreach (var item in selectedItems)
            {
                hatCollection.items.Remove(item);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item)); // Also delete the asset
                hatCollection.bodyParts = hatCollection.items; // Synchronize the lists
            }
            selectedItems.Clear(); // Clear the list of selected items

            // Refresh the ListView
            listViewContainer.Clear();
            PopulateListView(listViewContainer, hatCollection.bodyParts);
            EditorUtility.SetDirty(target); // Mark the asset as modified
        })
        {
            text = "Delete Selected Hat"
        };
        deleteButton.style.marginTop = 5;
        root.Add(deleteButton);

        // Add the container to the root visual element
        root.Add(listViewContainer);

        return root;
    }

    // Helper method to populate ListView
    private void PopulateListView(VisualElement container, List<Hat> items)
    {
        if (items == null || items.Count == 0)
        {
            container.Add(new Label("No hat items available. Populate your HatCollection asset."));
        }
        else
        {
            const int itemHeight = 20;
            Func<VisualElement> makeItem = () => new Label();
            Action<VisualElement, int> bindItem = (element, index) =>
            {
                (element as Label).text = items[index].hatName;
            };

            var listView = new ListView(items, itemHeight, makeItem, bindItem);
            listView.style.flexGrow = 1.0f;
            container.Add(listView);
        }
    }
}
