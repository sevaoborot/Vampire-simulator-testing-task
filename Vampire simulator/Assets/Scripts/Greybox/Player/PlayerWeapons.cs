using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeaponsReader
{
    public bool TryGetLevel(WeaponData data, out int level);
}

public class PlayerWeapons : IPlayerWeaponsReader
{
    private Transform _owner;
    private ViewportBounds _viewportBounds;
    private EventBus _eventBus;

    private Dictionary<WeaponData, Weapon> _currentWeapons = new Dictionary<WeaponData, Weapon>();

    public PlayerWeapons(ViewportBounds viewportBounds, Transform owner, WeaponData testWeapon, EventBus eventBus)
    {
        _eventBus = eventBus;

        _viewportBounds = viewportBounds;
        _owner = owner;
        AddWeapon(testWeapon);
        _eventBus.Subscribe<WeaponChosenSignal>(ReceiveNewWeapon);
    }

    public void Attack()
    {
        foreach (var weapon in _currentWeapons)
            weapon.Value.Attack();
    }

    private void ReceiveNewWeapon(WeaponChosenSignal signal) => AddWeapon(signal.WeaponData);

    public void AddWeapon(WeaponData weaponData) //not finished or tested yet
    {
        foreach(var weapon in _currentWeapons)
            if (weapon.Value.SameID(weaponData.weaponID))
            {
                weapon.Value.UpgrateWeapon();
                return;
            }
        _currentWeapons.Add(weaponData, new Weapon(weaponData, _owner, _viewportBounds));
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
