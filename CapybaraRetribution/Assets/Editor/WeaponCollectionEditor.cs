using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(WeaponCollection))]
public class WeaponCollectionEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // Create the root visual element
        var root = new VisualElement();

        // Add a simple label to test visibility
        var testLabel = new Label("Weapon Collection Settings");
        testLabel.style.fontSize = 14;
        testLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        testLabel.style.marginBottom = 10;
        root.Add(testLabel);

        // Retrieve the target WeaponCollection
        WeaponCollection weaponCollection = (WeaponCollection)target;

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

        // TextField for adding new Weapon items
        var newItemNameField = new TextField("New Weapon Name");
        newItemNameField.style.marginBottom = 8;
        root.Add(newItemNameField);

        // ObjectField for selecting a sprite for the new Weapon
        var newWeaponSpriteField = new ObjectField("New Weapon Sprite");
        newWeaponSpriteField.objectType = typeof(Sprite);
        newWeaponSpriteField.style.marginBottom = 8;
        root.Add(newWeaponSpriteField);

        // Add button for new Weapon items
        var addButton = new Button(() =>
        {
            // Create and add new Weapon item
            if (!string.IsNullOrWhiteSpace(newItemNameField.value))
            {
                // Ensure a path is set for saving the new Weapon ScriptableObject
                string path = EditorUtility.SaveFilePanelInProject("Save New Weapon", newItemNameField.value, "asset", "Please enter a file name to save the new weapon item.");
                if (string.IsNullOrEmpty(path))
                    return;

                Weapon newWeapon = CreateInstance<Weapon>();
                newWeapon.weaponName = newItemNameField.value;
                newWeapon.weaponSprite = (Sprite)newWeaponSpriteField.value;

                // Save the new Weapon as a ScriptableObject in the selected path
                AssetDatabase.CreateAsset(newWeapon, path);
                AssetDatabase.SaveAssets();

                weaponCollection.items.Add(newWeapon);
                weaponCollection.bodyParts = weaponCollection.items; // Synchronize the lists
                newItemNameField.value = "";
                newWeaponSpriteField.value = null;

                // Refresh the ListView
                listViewContainer.Clear();
                PopulateListView(listViewContainer, weaponCollection.bodyParts);
                EditorUtility.SetDirty(target); // Mark the asset as modified
            }
        })
        {
            text = "Add New Weapon"
        };
        root.Add(addButton);

        // List to keep track of selected items
        List<Weapon> selectedItems = new List<Weapon>();

        // ListView setup for Weapon items
        ListView listView = new ListView(weaponCollection.bodyParts, 20, () => new Label(), (element, index) =>
        {
            (element as Label).text = weaponCollection.bodyParts[index].weaponName;
            // Make the label clickable and navigate to the ScriptableObject when clicked
            element.RegisterCallback<ClickEvent>(evt =>
            {
                Selection.activeObject = weaponCollection.bodyParts[index];
                EditorGUIUtility.PingObject(weaponCollection.bodyParts[index]);
            });
        });
        listView.selectionType = SelectionType.Multiple;
        listView.onSelectionChange += selected =>
        {
            selectedItems.Clear();
            foreach (var item in selected)
                selectedItems.Add((Weapon)item);
        };
        listView.style.flexGrow = 1.0f;
        listViewContainer.Add(listView);

        // Delete button to remove selected Weapon items
        var deleteButton = new Button(() =>
        {
            foreach (var item in selectedItems)
            {
                weaponCollection.items.Remove(item);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item)); // Also delete the asset
                weaponCollection.bodyParts = weaponCollection.items; // Synchronize the lists
            }
            selectedItems.Clear(); // Clear the list of selected items

            // Refresh the ListView
            listViewContainer.Clear();
            PopulateListView(listViewContainer, weaponCollection.bodyParts);
            EditorUtility.SetDirty(target); // Mark the asset as modified
        })
        {
            text = "Delete Selected Weapon"
        };
        deleteButton.style.marginTop = 5;
        root.Add(deleteButton);

        // Add the container to the root visual element
        root.Add(listViewContainer);

        return root;
    }

    // Helper method to populate ListView
    private void PopulateListView(VisualElement container, List<Weapon> items)
    {
        if (items == null || items.Count == 0)
        {
            container.Add(new Label("No weapon items available. Populate your WeaponCollection asset."));
        }
        else
        {
            const int itemHeight = 20;
            Func<VisualElement> makeItem = () => new Label();
            Action<VisualElement, int> bindItem = (element, index) =>
            {
                (element as Label).text = items[index].weaponName;
            };

            var listView = new ListView(items, itemHeight, makeItem, bindItem);
            listView.style.flexGrow = 1.0f;
            container.Add(listView);
        }
    }
}
