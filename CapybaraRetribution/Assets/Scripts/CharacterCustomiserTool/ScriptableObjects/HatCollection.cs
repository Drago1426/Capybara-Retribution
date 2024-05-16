using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Character/HatCollection")]
public class HatCollection : BodyPartsCollections<Hat>
{
    [SerializeField]
    public List<Hat> bodyParts;

    private void OnValidate()
    {
        // Ensure bodyParts is synchronized with items on validation
        if (bodyParts == null)
            bodyParts = new List<Hat>();
        if (items == null)
            items = new List<Hat>();

        // Synchronize bodyParts with items if they differ
        if (bodyParts != items)
        {
            bodyParts.Clear();
            bodyParts.AddRange(items);
        }
    }
}
