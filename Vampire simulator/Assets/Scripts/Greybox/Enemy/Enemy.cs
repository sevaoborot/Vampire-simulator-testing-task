using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;

    public float Damage { get; private set; }

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;

    private Transform _player;
    private Action _poolRelease;

    public void Initialize(Transform player, Action poolRelease)
    {
        _player = player;
        _poolRelease = poolRelease;

        Damage = _enemyData.Damage;

        _enemyHealth = new EnemyHealth(_enemyData.MaxHealth);
        _enemyMovement = new EnemyMovement();

        _enemyHealth.OnDeath += Death;
    }

    private void Update()
    {
        if (_player != null && Time.timeScale != 0f) transform.position = _enemyMovement.Move(transform, _player, _enemyData.MaxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision) //should be changed to collision?
    {
        if (Time.timeScale != 0f)
        {
            if (collision.TryGetComponent<WeaponProjectile>(out WeaponProjectile weaponProjectile))
            {
                _enemyHealth.ReceiveDamage(weaponProjectile.Damage);
                weaponProjectile.RegisterEnemyHit();
            }
        }
    }

    private void Death()
    {
        _enemyHealth.OnDeath -= Death;
        _poolRelease();
    }
}
