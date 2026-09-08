using UnityEngine;

[CreateAssetMenu(fileName = "New regular damaging area", menuName = "Vampire Sim/New weapon/New damaging area weapon/New regular damaging area")]
public class RegularDamagingAreaWeaponData : DamageAreaWeaponData
{
    public RegularDamagingAreaWeaponLevels[] weaponLevels = new RegularDamagingAreaWeaponLevels[levelsNumber];

    public override ILevel GetLevel(int level) => weaponLevels[level];

    public override Weapon CreateWeapon(WeaponInfo weaponInfo) =>
        new RegularDamagingAreaWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds);
}

[System.Serializable]
public struct RegularDamagingAreaWeaponLevels : ILevel
{
    public string levelDescription;
    public string LevelDescription => levelDescription;

    public float areaSize;
    public float areaDamage;
    public int areasNumber;
    public float areaDamageCooldown; 
    public float areaSpawningCooldown;
    public float areaExistanceTime;
}
