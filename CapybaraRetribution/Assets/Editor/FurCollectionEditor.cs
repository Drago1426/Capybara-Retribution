using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FurCollection))]
public class FurCollectionEditor : Editor
{
    // List to keep track of selected items
    List<Fur> selectedItems = new List<Fur>();
    public override VisualElement CreateInspectorGUI()
    {
        // Create the root visual element
        var root = new VisualElement();

        // Add a simple label to test visibility
        var testLabel = new Label("Fur Collection Settings");
        testLabel.style.fontSize = 14;
        testLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        testLabel.style.marginBottom = 10;
        root.Add(testLabel);

        // Retrieve the target FurCollection
        FurCollection furCollection = (FurCollection)target;

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

        // TextField for adding new Fur items
        var newItemNameField = new TextField("New Fur Name");
        newItemNameField.style.marginBottom = 8;
        root.Add(newItemNameField);

        // ObjectField for selecting a sprite for the new Fur
        var newFurSpriteField = new ObjectField("New Fur Sprite");
        newFurSpriteField.objectType = typeof(Sprite);
        newFurSpriteField.style.marginBottom = 8;
        root.Add(newFurSpriteField);

        // Add button for new Fur items
        var addButton = new Button(() =>
        {
            // Create and add new Fur item
            if (!string.IsNullOrWhiteSpace(newItemNameField.value))
            {
                // Ensure a path is set for saving the new Fur ScriptableObject
                string path = EditorUtility.SaveFilePanelInProject("Save New Fur", newItemNameField.value, "asset", "Please enter a file name to save the new fur item.");
                if (string.IsNullOrEmpty(path))
                    return;

                Fur newFur = CreateInstance<Fur>();
                newFur.furName = newItemNameField.value;
                //newFur.furSprite = (Sprite)newFurSpriteField.value;

                // Save the new Fur as a ScriptableObject in the selected path
                AssetDatabase.CreateAsset(newFur, path);
                AssetDatabase.SaveAssets();

                furCollection.items.Add(newFur);
                furCollection.bodyParts = furCollection.items; // Synchronize the lists
                newItemNameField.value = "";
                newFurSpriteField.value = null;

                // Refresh the ListView
                listViewContainer.Clear();
                PopulateListView(listViewContainer, furCollection.bodyParts);
                EditorUtility.SetDirty(target); // Mark the asset as modified
            }
        })
        {
            text = "Add New Fur"
        };
        root.Add(addButton);

        // // List to keep track of selected items
        // List<Fur> selectedItems = new List<Fur>();
        //
        // // ListView setup for Fur items
        // ListView listView = new ListView(furCollection.bodyParts, 20, () => new Label(), (element, index) =>
        // {
        //     (element as Label).text = furCollection.bodyParts[index].furName;
        //     // Make the label clickable and navigate to the ScriptableObject when clicked
        //     element.RegisterCallback<ClickEvent>(evt =>
        //     {
        //         Selection.activeObject = furCollection.bodyParts[index];
        //         EditorGUIUtility.PingObject(furCollection.bodyParts[index]);
        //     });
        // });
        // listView.selectionType = SelectionType.Multiple;
        // listView.selectionChanged += selected =>
        // {
        //     selectedItems.Clear();
        //     foreach (var item in selected)
        //         selectedItems.Add((Fur)item);
        // };
        // listView.style.flexGrow = 1.0f;
        // listViewContainer.Add(listView);

        PopulateListView(listViewContainer, furCollection.bodyParts);

        // Delete button to remove selected Fur items
        var deleteButton = new Button(() =>
        {
            foreach (var item in selectedItems)
            {
                furCollection.items.Remove(item);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item)); // Also delete the asset
                furCollection.bodyParts = furCollection.items; // Synchronize the lists
            }
            selectedItems.Clear(); // Clear the list of selected items

            // Refresh the ListView
            listViewContainer.Clear();
            PopulateListView(listViewContainer, furCollection.bodyParts);
            EditorUtility.SetDirty(target); // Mark the asset as modified
        })
        {
            text = "Delete Selected Fur"
        };
        deleteButton.style.marginTop = 5;
        root.Add(deleteButton);

        // Add the container to the root visual element
        root.Add(listViewContainer);

        return root;
    }

    // Helper method to populate ListView
    private void PopulateListView(VisualElement container, List<Fur> items)
    {
        if (items == null || items.Count == 0)
        {
            container.Add(new Label("No fur items available. Populate your FurCollection asset."));
        }
        else
        {
            const int itemHeight = 20;
            Func<VisualElement> makeItem = () => new Label();
            Action<VisualElement, int> bindItem = (element, index) =>
            {
                (element as Label).text = items[index].furName;
            };

            var listView = new ListView(items, itemHeight, makeItem, bindItem);
            listView.selectionChanged += selected =>
            {
                selectedItems.Clear();
                foreach (var item in selected)
                    selectedItems.Add((Fur)item);
            };
            listView.style.flexGrow = 1.0f;
            container.Add(listView);
        }
    }
}
