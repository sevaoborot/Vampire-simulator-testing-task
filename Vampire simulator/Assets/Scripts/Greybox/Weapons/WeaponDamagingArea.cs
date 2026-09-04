using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamagingArea : MonoBehaviour
{
    public float Damage { get; private set; }

    private float _areaExistanceTime;
    private float _areaDamageCooldown;

    private Dictionary<EnemyHealth, Coroutine> _enemiesInArea = new Dictionary<EnemyHealth, Coroutine>();

    public void Initialize(float areaDamage, float areaExistanceTime, float areaDamageCooldown)
    {
        Damage = areaDamage;
        _areaDamageCooldown = areaDamageCooldown;
        _areaExistanceTime = areaExistanceTime;

        if (_areaExistanceTime > 0) StartCoroutine(AreaExisting()); 
    }

    public void EnterArea(EnemyHealth enemyHealth)
    {
        RegisterInArea(enemyHealth);
    }

    public void LeaveArea(EnemyHealth enemyHealth)
    {
        DeregisterFromArea(enemyHealth);
    }

    private void RegisterInArea(EnemyHealth enemyHealth)
    {
        _enemiesInArea.Add(enemyHealth, StartCoroutine(DamageCooldown(enemyHealth)));
    }

    private void DeregisterFromArea(EnemyHealth enemyHealth) //also should be called when enemy death in area
    {
        if (_enemiesInArea.TryGetValue(enemyHealth, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            _enemiesInArea.Remove(enemyHealth);
        }
    }

    private IEnumerator DamageCooldown(EnemyHealth enemyHealth)
    {
        while (true)
        {
            enemyHealth.ReceiveDamage(Damage);
            yield return new WaitForSeconds(_areaDamageCooldown);
        }
    }

    private IEnumerator AreaExisting() //should be renamed
    {
        yield return new WaitForSeconds(_areaExistanceTime);
        foreach (var enemy in _enemiesInArea) StopCoroutine(enemy.Value);
        _enemiesInArea.Clear();
    }
}
