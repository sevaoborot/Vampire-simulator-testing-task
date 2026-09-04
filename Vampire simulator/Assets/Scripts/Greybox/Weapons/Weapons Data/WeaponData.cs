using UnityEngine;

public abstract class WeaponData : ScriptableObject //need different SO for projectile and damaging area weapons
{
    public string weaponID;
    public GameObject projectile;
    public static int levelsNumber = 8; //8 levels for eeach weapon

    public abstract ILevel GetLevel(int level);
    public abstract Weapon CreateWeapon(WeaponInfo weaponInfo);
}

public interface ILevel
{
    public string LevelDescription { get; }
}