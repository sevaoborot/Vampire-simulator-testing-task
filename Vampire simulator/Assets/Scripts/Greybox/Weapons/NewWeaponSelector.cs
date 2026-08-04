using System.Collections.Generic;
using UnityEngine;

public class NewWeaponSelector
{
    private EventBus _eventBus;
    private IPlayerWeaponsReader _playerWeaponsReader;

    public NewWeaponSelector(EventBus eventBus, IPlayerWeaponsReader playerWeaponsReader)
    {
        _eventBus = eventBus;
        _playerWeaponsReader = playerWeaponsReader;
    }

    public WeaponData RandomData(List<WeaponData> weaponOptions, out int level)
    {
        int i = Random.Range(0, weaponOptions.Count);
        WeaponData data = weaponOptions[i];
        if (!_playerWeaponsReader.TryGetLevel(data, out int currentLevel))
        {
            level = 0;
            return data;
        }
        int nextLevel = currentLevel + 1;
        if (nextLevel < data.weaponLevels.Length)
        {
            level = nextLevel;
            return data;
        }
        return RandomData(weaponOptions, out level);
    }

    public void RegisterChosenWeapon(WeaponData data)
    {
        _eventBus.Invoke(new WeaponChosenSignal(data));
    }
}
