using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection", menuName = "Character/FurCollection")]
public class FurCollection : BodyPartsCollections<Fur>
{
    [SerializeField] public IEnumerable<Fur> bodyParts;
}