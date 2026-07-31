using System;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public Vector2 projectileVelocity {  get; private set; }
    public float Damage { get; private set; }

    private IProjectileMovement _movement;
    private ViewportBounds _viewportBounds;
    private Action _poolRelease;
    //private bool _isInit = false;

    public void Initialize(ViewportBounds viewportBounds, IProjectileMovement projectileMovement, Action projectileRelease, float projectileDamage)
    {
        _movement = projectileMovement;
        _viewportBounds = viewportBounds;
        _poolRelease = projectileRelease;
        Damage = projectileDamage;

        //_isInit = true;
    }

    private void Update()
    {
        if (_movement != null) _movement.Move(transform);
    }

    private void LateUpdate()
    {
        if (!CheckProjectileVisibility()) ReleaseProjectile();
    }

    public void ReleaseProjectile()
    {
        //_isInit = false;
        _poolRelease();
    }

    private bool CheckProjectileVisibility()
    {
        Vector2 currentPosition = transform.position;
        return _viewportBounds.viewportRect.Contains(currentPosition);
    }
}
