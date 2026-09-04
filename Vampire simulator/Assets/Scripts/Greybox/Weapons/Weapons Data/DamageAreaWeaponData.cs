using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Vampire Sim/New weapon/New damaging area weapon")]
public class DamageAreaWeaponData : WeaponData
{
    public DamageAreaWeaponLevels[] weaponLevels = new DamageAreaWeaponLevels[levelsNumber];

    public override ILevel GetLevel(int level) => weaponLevels[level];

    public override Weapon CreateWeapon(WeaponInfo weaponInfo) =>
        new DamagingAreaWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds);
}

[System.Serializable]
public struct DamageAreaWeaponLevels : ILevel
{
    public string levelDescription;
    public string LevelDescription => levelDescription;

    public float areaSize;
    public float areaDamage;
    public int areasNumber;
    public float areaCooldown; //rename to areaDamageCooldown
    public float areaExistanceTime;
}