using UnityEngine;

[CreateAssetMenu(fileName = "New always enabled damaging area", menuName = "Vampire Sim/New weapon/New damaging area weapon/New always enabled damaging area")]
public class AlwaysEnabledDamagingAreaWeaponData : DamageAreaWeaponData
{
    public AlwaysEnabledDamagingAreaWeaponLevels[] weaponLevels = new AlwaysEnabledDamagingAreaWeaponLevels[levelsNumber];

    public override ILevel GetLevel(int level) => weaponLevels[level];

    public override Weapon CreateWeapon(WeaponInfo weaponInfo) =>
        new AlwaysEnabledDamagingAreaWeapon(this, weaponInfo.ownerTransform, weaponInfo.viewportBounds);
}

[System.Serializable]
public struct AlwaysEnabledDamagingAreaWeaponLevels : ILevel
{
    public string levelDescription;
    public string LevelDescription => levelDescription;

    public float areaSize;
    public float areaDamage;
    public float areaDamageCooldown; 
}
