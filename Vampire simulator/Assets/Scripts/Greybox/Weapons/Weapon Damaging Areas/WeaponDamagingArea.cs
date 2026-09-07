using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDamagingArea : MonoBehaviour
{
    public float Damage { get; protected set; }
    protected float _areaDamageCooldown; 
    protected IDamagingAreaMovement _areaMovement;

    protected Dictionary<EnemyHealth, Coroutine> _enemiesInArea = new Dictionary<EnemyHealth, Coroutine>();

    protected void ProtectedInitialize(IDamagingAreaMovement areaMovement, float areaDamage, float areaDamageCooldown)
    {
        Damage = areaDamage;
        _areaMovement = areaMovement;
        _areaDamageCooldown = areaDamageCooldown;
    }

    private void Update()
    {
        if (_areaMovement != null && Time.timeScale != 0f) transform.position = _areaMovement.Move();
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
}
