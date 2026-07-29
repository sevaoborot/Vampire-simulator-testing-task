using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Vampire Sim/New weapon")]
public class WeaponData : ScriptableObject
{
    public GameObject projectile;
    public int weaponLevel;
    public float projectileSpeed;
    public float projectileDamage;
    public int projectileNumber;
    public float weaponCooldown;
    public ProjectileBehavioursEnum projectileBehaviour;
}
