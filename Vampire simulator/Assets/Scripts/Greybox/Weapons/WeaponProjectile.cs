using System;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public Vector2 projectileVelocity {  get; private set; }
    public float Damage { get; private set; }

    private IProjectileMovement _movement;
    private ViewportBounds _viewportBounds;
    private Action _poolRelease;
    private int _maxEnemiesToHit;
    private int _currentEnemies;
    private bool _IsMaxEnemiesHit => _currentEnemies == _maxEnemiesToHit;

    public void Initialize(ViewportBounds viewportBounds, IProjectileMovement projectileMovement, Action projectileRelease, float projectileDamage, int maxEnemiesToHit)
    {
        _movement = projectileMovement;
        _viewportBounds = viewportBounds;
        _poolRelease = projectileRelease;
        Damage = projectileDamage;
        _maxEnemiesToHit = maxEnemiesToHit;
        _currentEnemies = 0;
    }

    private void Update()
    {
        if (_movement != null && Time.timeScale != 0f) _movement.Move(transform);
    }

    private void LateUpdate()
    {
        if (_IsMaxEnemiesHit || !CheckProjectileVisibility()) ReleaseProjectile();
    }

    private void ReleaseProjectile()
    {
        _poolRelease();
    }

    public void RegisterEnemyHit() => _currentEnemies++;

    private bool CheckProjectileVisibility()
    {
        Vector2 currentPosition = transform.position;
        return _viewportBounds.viewportRect.Contains(currentPosition);
    }
}
