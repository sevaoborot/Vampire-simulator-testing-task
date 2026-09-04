using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Vampire Sim/New weapon/New projectile weapon")]
public class ProjectileWeaponData : WeaponData
{
    public ProjectileBehavioursEnum projectileBehaviour;

    public ProjectileWeaponLevels[] weaponLevels = new ProjectileWeaponLevels[levelsNumber];

    public override ILevel GetLevel(int level) => weaponLevels[level];

    public override Weapon CreateWeapon(WeaponInfo weaponInfo) =>
        new ProjectileWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds, weaponInfo.directionReader);
}

[System.Serializable]
public struct ProjectileWeaponLevels : ILevel
{
    public string levelDescription;
    public string LevelDescription => levelDescription;

    public float projectileSpeed;
    public float projectileDamage;
    public int projectileNumber;
    public float weaponCooldown;
    public int maxEnemiesToHit;
}