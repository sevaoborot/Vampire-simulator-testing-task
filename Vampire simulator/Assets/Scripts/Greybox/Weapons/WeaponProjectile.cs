using System;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public Vector2 projectileVelocity {  get; private set; }
    public float projectileDamage { get; private set; }

    private IProjectileMovement _projectileMovement;
    private ViewportBounds _viewportBounds;
    private Action _projectileRelease;

    public void Initialize(ViewportBounds viewportBounds, IProjectileMovement projectileMovement, Action projectileRelease, float projectileDamage)
    {
        _projectileMovement = projectileMovement;
        _viewportBounds = viewportBounds;
        _projectileRelease = projectileRelease;

        this.projectileDamage = projectileDamage;
    }

    private void Update()
    {
        if (_projectileMovement != null) _projectileMovement.Move(transform);
    }

    private void LateUpdate()
    {
        if (!CheckProjectileVisibility()) ReleaseProjectile();
    }

    public void ReleaseProjectile() => _projectileRelease();

    private bool CheckProjectileVisibility()
    {
        Vector2 currentPosition = transform.position;
        return _viewportBounds.viewportRect.Contains(currentPosition);
    }
}
