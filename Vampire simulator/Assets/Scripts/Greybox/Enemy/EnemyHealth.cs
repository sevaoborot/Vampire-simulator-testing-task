using System;
using UnityEngine;

public class EnemyHealth //: MonoBehaviour
{
    private float _enemyCurrentHealth;
    private float _enemyMaxHealth;

    //same not sure as in 'PlayerHealth.cs'
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public EnemyHealth(float enemyMaxHealth)
    {
        _enemyMaxHealth = enemyMaxHealth;
        _enemyCurrentHealth = _enemyMaxHealth;
        Debug.Log($"Current enemy health is {_enemyCurrentHealth}");
    }

    public float EnemyCurrentHealth
    {
        get => _enemyCurrentHealth;
        private set
        {
            float clamped = Mathf.Clamp(value, 0f, _enemyMaxHealth);
            if (Mathf.Approximately(clamped, _enemyCurrentHealth)) return;
            _enemyCurrentHealth = clamped;
            if (_enemyCurrentHealth <= 0)
            {
                OnDeath?.Invoke();
                return;
            }
            OnHealthChanged?.Invoke(_enemyCurrentHealth, _enemyMaxHealth);
        }
    }

    public void OnDamageDealed(float amount)
    {
        EnemyCurrentHealth -= amount;
    }
}
