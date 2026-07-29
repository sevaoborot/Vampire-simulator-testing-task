using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _enemyCurrentHealth;
    [SerializeField] private float _enemyMaxHealth;

    //same not sure as in 'PlayerHealth.cs'
    public event Action<float, float> OnEnemyHealthChanged;

    private float EnemyCurrentHealth
    {
        get => _enemyCurrentHealth;
        set
        {
            float clamped = Mathf.Clamp(value, 0f, _enemyMaxHealth);
            if (Mathf.Approximately(clamped, _enemyCurrentHealth)) return;
            _enemyCurrentHealth = clamped;
            OnEnemyHealthChanged?.Invoke(_enemyCurrentHealth, _enemyMaxHealth);
            if (_enemyCurrentHealth <= 0) Debug.Log("Enemy is dead!");
        }
    }

    public void OnDamageDealed(float amount)
    {
        EnemyCurrentHealth -= amount;
    }

    public void OnHealthRestored(float amount)
    {
        EnemyCurrentHealth += amount;
    }
}
