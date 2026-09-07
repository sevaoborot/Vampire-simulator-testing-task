using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Vampire Sim/New weapon/New damaging area weapon")]
public class DamageAreaWeaponData : WeaponData
{
    public DamagingAreaMovementsEnum areaMovementType;
    public DamagingAreaSpawningEnum areaSpawning;
    public DamageAreaWeaponLevels[] weaponLevels = new DamageAreaWeaponLevels[levelsNumber];

    public override ILevel GetLevel(int level) => weaponLevels[level];

    public override Weapon CreateWeapon(WeaponInfo weaponInfo)
    {
        //i don't like if-case below but i guess it's ok for now
        if (weaponLevels[0].areaExistanceTime == 0 && weaponLevels[0].areaSpawningCooldown == 0) return new AlwaysEnabledDamagingAreaWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds);
        else return new RegularDamagingAreaWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds);
    }
}

[System.Serializable]
public struct DamageAreaWeaponLevels : ILevel
{
    public string levelDescription;
    public string LevelDescription => levelDescription;

    public float areaSize;
    public float areaDamage;
    public int areasNumber;
    public float areaDamageCooldown; //rename to areaDamageCooldown
    public float areaSpawningCooldown;
    public float areaExistanceTime;
}