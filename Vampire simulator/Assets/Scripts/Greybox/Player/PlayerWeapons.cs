using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeaponsReader
{
    public bool TryGetLevel(WeaponData data, out int level);
}

public class PlayerWeapons : IPlayerWeaponsReader
{
    private MonoBehaviour _owner;
    private EventBus _eventBus;

    private WeaponInfo _weaponInfo;

    private Dictionary<WeaponData, Weapon> _currentWeapons = new Dictionary<WeaponData, Weapon>();

    public PlayerWeapons(ViewportBounds viewportBounds, Transform ownerTransform, MonoBehaviour owner, WeaponData testWeapon, EventBus eventBus, IPlayerDirectionReader directionReader)
    {
        _eventBus = eventBus;

        _weaponInfo.ownerTransform = ownerTransform;
        _weaponInfo.viewportBounds = viewportBounds;
        _weaponInfo.directionReader = directionReader;

        _owner = owner;

        AddWeapon(testWeapon);
        _eventBus.Subscribe<WeaponChosenSignal>(ReceiveNewWeapon);
    }

    public void Attack()
    {
        foreach (var weapon in _currentWeapons)
            weapon.Value.Attack(_owner); 
    }

    private void ReceiveNewWeapon(WeaponChosenSignal signal) => AddWeapon(signal.WeaponData);

    public void AddWeapon(WeaponData weaponData) 
    {
        foreach(var weapon in _currentWeapons)
            if (weapon.Value.SameID(weaponData.weaponID))
            {
                weapon.Value.UpgrateWeapon();
                return;
            }
        _currentWeapons.Add(weaponData, weaponData.CreateWeapon(_weaponInfo)); 
    }

    public bool TryGetLevel(WeaponData data, out int level)
    {
        if (_currentWeapons.TryGetValue(data, out Weapon weapon)) {
            level = weapon.CurrentLevel;
            return true;
        }
        level = -1;
        return false;
    }
}

public struct WeaponInfo //should be renamed
{
    public Transform ownerTransform;
    public ViewportBounds viewportBounds;
    public IPlayerDirectionReader directionReader;
}