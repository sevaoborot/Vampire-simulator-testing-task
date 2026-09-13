using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] private EnemyData _enemyData;

    public float Damage { get; private set; }

    private EnemyHealth _enemyHealth;
    private EnemyMovement _enemyMovement;

    private float _maxSpeed;
    private Transform _player;
    private Action _poolRelease;

    public void Initialize(Transform player, float damage, float maxHealth, float maxSpeed, Action poolRelease)
    {
        _player = player;
        _poolRelease = poolRelease;

        Damage = damage;

        _enemyHealth = new EnemyHealth(maxHealth);

        _enemyMovement = new EnemyMovement();
        _maxSpeed = maxSpeed;

        _enemyHealth.OnDeath += Death;
    }

    private void Update()
    {
        if (_player != null && Time.timeScale != 0f) transform.position = _enemyMovement.Move(transform, _player, _maxSpeed);
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
            if (collision.TryGetComponent<SpawnedDamagingArea>(out SpawnedDamagingArea weaponDamagingArea))
                weaponDamagingArea.EnterArea(_enemyHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Time.timeScale != 0f)
        {
            if (collision.TryGetComponent<SpawnedDamagingArea>(out SpawnedDamagingArea weaponDamagingArea))
                weaponDamagingArea.LeaveArea(_enemyHealth);
        }
    }

    //should I add onTriggerEnd2D ???

    private void Death()
    {
        _enemyHealth.OnDeath -= Death;
        _poolRelease();
    }
}
