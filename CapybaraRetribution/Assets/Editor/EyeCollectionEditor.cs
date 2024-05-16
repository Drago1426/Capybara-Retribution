using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(EyeCollection))]
public class EyeCollectionEditor : Editor
{
    // List to keep track of selected items
    List<Eyes> selectedItems = new List<Eyes>();
    
    public override VisualElement CreateInspectorGUI()
    {
        // Create the root visual element
        var root = new VisualElement();

        // Add a simple label to test visibility
        var testLabel = new Label("Eye Collection Settings");
        testLabel.style.fontSize = 14;
        testLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        testLabel.style.marginBottom = 10;
        root.Add(testLabel);

        // Retrieve the target EyeCollection
        EyeCollection eyeCollection = (EyeCollection)target;

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

        // TextField for adding new Eye items
        var newItemNameField = new TextField("New Eye Name");
        newItemNameField.style.marginBottom = 8;
        root.Add(newItemNameField);

        // ObjectField for selecting a sprite for the new Eye
        var newEyeSpriteField = new ObjectField("New Eye Sprite");
        newEyeSpriteField.objectType = typeof(Sprite);
        newEyeSpriteField.style.marginBottom = 8;
        root.Add(newEyeSpriteField);

        // Add button for new Eye items
        var addButton = new Button(() =>
        {
            // Create and add new Eye item
            if (!string.IsNullOrWhiteSpace(newItemNameField.value))
            {
                // Ensure a path is set for saving the new Eye ScriptableObject
                string path = EditorUtility.SaveFilePanelInProject("Save New Eye", newItemNameField.value, "asset", "Please enter a file name to save the new eye item.");
                if (string.IsNullOrEmpty(path))
                    return;

                Eyes newEye = CreateInstance<Eyes>();
                newEye.eyesName = newItemNameField.value;
                newEye.eyesSprite = (Sprite)newEyeSpriteField.value;

                // Save the new Eye as a ScriptableObject in the selected path
                AssetDatabase.CreateAsset(newEye, path);
                AssetDatabase.SaveAssets();

                eyeCollection.items.Add(newEye);
                eyeCollection.bodyParts = eyeCollection.items; // Synchronize the lists
                newItemNameField.value = "";
                newEyeSpriteField.value = null;

                // Refresh the ListView
                listViewContainer.Clear();
                PopulateListView(listViewContainer, eyeCollection.bodyParts);
                EditorUtility.SetDirty(target); // Mark the asset as modified
            }
        })
        {
            text = "Add New Eye"
        };
        root.Add(addButton);


        // ListView setup for Eye items
        // ListView listView = new ListView(eyeCollection.bodyParts, 20, () => new Label(), (element, index) =>
        // {
        //     (element as Label).text = eyeCollection.bodyParts[index].eyesName;
        //     // Make the label clickable and navigate to the ScriptableObject when clicked
        //     element.RegisterCallback<ClickEvent>(evt =>
        //     {
        //         Selection.activeObject = eyeCollection.bodyParts[index];
        //         EditorGUIUtility.PingObject(eyeCollection.bodyParts[index]);
        //     });
        // });
        // listView.selectionType = SelectionType.Multiple;
        
        
        PopulateListView(listViewContainer, eyeCollection.bodyParts);

        // Delete button to remove selected Eye items
        var deleteButton = new Button(() =>
        {
            foreach (var item in selectedItems)
            {
                eyeCollection.items.Remove(item);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item)); // Also delete the asset
                eyeCollection.bodyParts = eyeCollection.items; // Synchronize the lists
            }
            selectedItems.Clear(); // Clear the list of selected items

            // Refresh the ListView
            listViewContainer.Clear();
            PopulateListView(listViewContainer, eyeCollection.bodyParts);
            EditorUtility.SetDirty(target); // Mark the asset as modified
        })
        {
            text = "Delete Selected Eye"
        };
        deleteButton.style.marginTop = 5;
        root.Add(deleteButton);

        // Add the container to the root visual element
        root.Add(listViewContainer);

        return root;
    }

    // Helper method to populate ListView
    private void PopulateListView(VisualElement container, List<Eyes> items)
    {
        if (items == null || items.Count == 0)
        {
            container.Add(new Label("No eye items available. Populate your EyeCollection asset."));
        }
        else
        {
            const int itemHeight = 20;
            Func<VisualElement> makeItem = () => new Label();
            Action<VisualElement, int> bindItem = (element, index) =>
            {
                (element as Label).text = items[index].eyesName;
            };

            var listView = new ListView(items, itemHeight, makeItem, bindItem);
            listView.selectionChanged += selected =>
            {
                selectedItems.Clear();
                foreach (var item in selected)
                    selectedItems.Add((Eyes)item);
            };
            listView.style.flexGrow = 1.0f;
            container.Add(listView);
        }
    }
}
