using UnityEngine;

public class WeaponProjectileMovement : MonoBehaviour
{
    private IProjectileMovement _projectileMovement;

    public void Initialize(IProjectileMovement projectileMovement) => _projectileMovement = projectileMovement;

    private void Update() => _projectileMovement.Move(transform);
}
