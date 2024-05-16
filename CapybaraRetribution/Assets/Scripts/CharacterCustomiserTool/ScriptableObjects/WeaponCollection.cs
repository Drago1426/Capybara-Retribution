using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Character/WeaponCollection")]
public class WeaponCollection : BodyPartsCollections<Weapon>
{
    [SerializeField]
    public List<Weapon> bodyParts;

    private void OnValidate()
    {
        // Ensure bodyParts is synchronized with items on validation
        if (bodyParts == null)
            bodyParts = new List<Weapon>();
        if (items == null)
            items = new List<Weapon>();

        // Synchronize bodyParts with items if they differ
        if (bodyParts != items)
        {
            bodyParts.Clear();
            bodyParts.AddRange(items);
        }
    }
}
