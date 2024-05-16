using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Character/FurCollection")]
public class FurCollection : BodyPartsCollections<Fur>
{
    [SerializeField]
    public List<Fur> bodyParts;

    private void OnValidate()
    {
        // Ensure bodyParts is synchronized with items on validation
        if (bodyParts == null)
            bodyParts = new List<Fur>();
        if (items == null)
            items = new List<Fur>();

        // Synchronize bodyParts with items if they differ
        if (bodyParts != items)
        {
            bodyParts.Clear();
            bodyParts.AddRange(items);
        }
    }
}
