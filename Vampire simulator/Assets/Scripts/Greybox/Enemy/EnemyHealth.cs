using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _enemyCurrentHealth;
    [SerializeField] private float _enemyMaxHealth;
    private Action _releaseEnemy;

    //same not sure as in 'PlayerHealth.cs'
    public event Action<float, float> OnEnemyHealthChanged;

    public void Initialize(Action releaseEnemy)
    {
        _releaseEnemy = releaseEnemy;
        _enemyCurrentHealth = _enemyMaxHealth;
    }

    public float EnemyCurrentHealth
    {
        get => _enemyCurrentHealth;
        private set
        {
            float clamped = Mathf.Clamp(value, 0f, _enemyMaxHealth);
            if (Mathf.Approximately(clamped, _enemyCurrentHealth)) return;
            _enemyCurrentHealth = clamped;
            OnEnemyHealthChanged?.Invoke(_enemyCurrentHealth, _enemyMaxHealth);
            if (_enemyCurrentHealth <= 0) OnDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<WeaponProjectile>(out WeaponProjectile projectile))
        {
            Debug.Log("Enemy recieved damage");
            OnDamageDealed(projectile.projectileDamage);
        }
    }

    public void OnDamageDealed(float amount)
    {
        EnemyCurrentHealth -= amount;
    }

    private void OnDeath()
    {
        Debug.Log("Enemy is dead!");
        _releaseEnemy();
    }

    //I dont thibk enemies should have an ability to restore their health

    //public void OnHealthRestored(float amount)
    //{
    //    EnemyCurrentHealth += amount;
    //}
}
