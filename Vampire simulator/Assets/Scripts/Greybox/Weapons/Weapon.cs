using UnityEngine;

public class Weapon 
{
    private GameObject _projectile; //+
    private int _weaponLevel;
    private float _projectileSpeed; //+
    private float _projectileDamage;
    private int _projectileNumber;
    private float _weaponCooldown; //+
    private ProjectileBehavioursEnum _projectileBehaviour; //+

    private Transform _playerTransform;
    private float _weaponCooldownEndTime;

    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    public Weapon(WeaponData weaponData, Transform playerTransform)
    {
        _projectile = weaponData.projectile;
        _weaponLevel = weaponData.weaponLevel;
        _projectileSpeed = weaponData.projectileSpeed;
        _projectileDamage = weaponData.projectileDamage;
        _projectileNumber = weaponData.projectileNumber;
        _weaponCooldown = weaponData.weaponCooldown;
        _projectileBehaviour = weaponData.projectileBehaviour;

        _playerTransform = playerTransform;
    }

    public void Attack()
    {
        if (!_canAttack) return;
        GameObject newProjectile = Object.Instantiate(_projectile, _playerTransform.position, Quaternion.identity);
        newProjectile.GetComponent<WeaponProjectileMovement>().Initialize(ChooseProjectileMovement(_projectileBehaviour));
        Debug.Log("Shooting...");
        StartCooldown();

        //stop cooldowns
    }

    private void StartCooldown()
    {
        _weaponCooldownEndTime = Time.time + _weaponCooldown;
    }

    private void InterruptCooldown()
    {
        _weaponCooldownEndTime = Time.time;
    }

    private IProjectileMovement ChooseProjectileMovement(ProjectileBehavioursEnum projectileBehaviour)
    {
        switch (projectileBehaviour)
        {
            case ProjectileBehavioursEnum.ProjectileFollowsEnemyMovement:
                return new ProjectileFollowsEnemyMovement(_playerTransform, _projectileSpeed);
            default:
                Debug.LogError("No movement logic for projectile! Returning null!");
                return null;
        }
    }
}
