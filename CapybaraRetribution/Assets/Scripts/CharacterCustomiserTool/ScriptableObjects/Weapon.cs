using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Character/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public int damage;
}