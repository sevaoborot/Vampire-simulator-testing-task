public abstract class DamageAreaWeaponData : WeaponData
{
    public DamagingAreaMovementsEnum areaMovementType;
    public DamagingAreaSpawningEnum areaSpawning;
    public override ILevel GetLevel(int level) => null; //i dont like it but i guess it's ok for now

    public override Weapon CreateWeapon(WeaponInfo weaponInfo) => null;
}