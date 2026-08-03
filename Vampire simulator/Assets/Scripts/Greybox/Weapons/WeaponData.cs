using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Vampire Sim/New weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponID;

    public GameObject projectile;
    public ProjectileBehavioursEnum projectileBehaviour;
    public WeaponLevels[] weaponLevels = new WeaponLevels[8]; 
}

[System.Serializable]
public struct WeaponLevels
{
    public string levelDescription;

    //public int weaponLevel;
    public float projectileSpeed;
    public float projectileDamage;
    public int projectileNumber;
    public float weaponCooldown;
}
