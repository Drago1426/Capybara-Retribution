using UnityEngine;

public enum WeaponType
{
    None,
    Gun,
    Purse
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Customization/Weapon" )]
public class Weapon : ScriptableObject
{
    public WeaponType weaponType = WeaponType.None;
    public GameObject weaponPrefab;
}