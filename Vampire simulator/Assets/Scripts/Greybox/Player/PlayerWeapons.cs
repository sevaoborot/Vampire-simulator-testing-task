using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponData _test;

    private ViewportBounds _viewportBounds;
    private Dictionary<WeaponData, Weapon> _currentWeapons = new Dictionary<WeaponData, Weapon>();

    public void Initialize(ViewportBounds viewportBounds)
    {
        _viewportBounds = viewportBounds;
        AddWeapon(_test);
    }

    private void Update()
    {
        foreach (var weapon in _currentWeapons)
            weapon.Value.Attack();
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if (_currentWeapons.TryAdd(weaponData, new Weapon(weaponData, transform, _viewportBounds))) return;
        else
        {
            Debug.Log("This weapon has been added to the inventory already. Increasing its lvl...");
            //increasing lvl...
        }
    }
}
