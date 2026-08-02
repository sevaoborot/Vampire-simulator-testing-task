using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons 
{
    //private WeaponData _testWeapon;
    private Transform _owner;

    private ViewportBounds _viewportBounds;
    private Dictionary<WeaponData, Weapon> _currentWeapons = new Dictionary<WeaponData, Weapon>();

    public PlayerWeapons(ViewportBounds viewportBounds, Transform owner, WeaponData testWeapon)
    {
        _viewportBounds = viewportBounds;
        _owner = owner;
        AddWeapon(testWeapon);
    }

    public void Attack()
    {
        foreach (var weapon in _currentWeapons)
            weapon.Value.Attack(); 
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if (_currentWeapons.TryAdd(weaponData, new Weapon(weaponData, _owner, _viewportBounds))) return;
        else
        {
            Debug.Log("This weapon has been added to the inventory already. Increasing its lvl...");
            //increasing lvl...
        }
    }
}
