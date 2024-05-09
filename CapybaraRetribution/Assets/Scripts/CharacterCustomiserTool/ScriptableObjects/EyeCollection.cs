using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Character/EyeCollection")]
public class EyeCollection : BodyPartsCollections<Eyes>
{
    [SerializeField]
    public List<Eyes> bodyParts;

    private void OnValidate()
    {
        // Ensure bodyParts is synchronized with items on validation
        if (bodyParts == null)
            bodyParts = new List<Eyes>();
        if (items == null)
            items = new List<Eyes>();

        // Synchronize bodyParts with items if they differ
        if (bodyParts != items)
        {
            bodyParts.Clear();
            bodyParts.AddRange(items);
        }
    }
}