using UnityEngine;
public enum AccessoryType
{
    None,
    FunnyHat,
    Fez,
    Fedora,
    Beret
}

[CreateAssetMenu(fileName = "Accessories", menuName = "Customization/Accessories")]
public class Accessories : ScriptableObject
{
    public AccessoryType accessoryType = AccessoryType.None;
    public GameObject[] accessoryPrefabs;
}