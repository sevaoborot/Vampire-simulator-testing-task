using System;
using System.Collections;
using UnityEngine;

public class WeaponRegularDamagingArea : WeaponDamagingArea
{
    private float _areaExistanceTime;
    private Action _onAreaRelease;

    public void Initialize(IDamagingAreaMovement areaMovement, float areaDamage, float areaExistanceTime, float areaDamageCooldown, Action onAreaRelease)
    {
        ProtectedInitialize(areaMovement, areaDamage, areaDamageCooldown);
        _onAreaRelease = onAreaRelease;
        _areaExistanceTime = areaExistanceTime;

        StartCoroutine(AreaExisting());
    }

    private IEnumerator AreaExisting() //should be renamed
    {
        yield return new WaitForSeconds(_areaExistanceTime);
        foreach (var enemy in _enemiesInArea) StopCoroutine(enemy.Value);
        _enemiesInArea.Clear();
        _onAreaRelease();
    }
}
