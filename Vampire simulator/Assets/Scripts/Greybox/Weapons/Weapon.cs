using UnityEngine;

public class Weapon 
{
    private GameObject _projectile; //+
    private int _weaponLevel;
    private float _projectileSpeed; //+
    private float _projectileDamage; //+
    private int _projectileNumber;
    private float _weaponCooldown; //+
    private ProjectileBehavioursEnum _projectileBehaviour; //+

    private Transform _playerTransform;
    private CustomObjectPool _projectilesPool;
    private float _weaponCooldownEndTime;
    private ViewportBounds _viewportBounds;

    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    public Weapon(WeaponData weaponData, Transform playerTransform, ViewportBounds viewportBounds)
    {
        _projectile = weaponData.projectile;
        _weaponLevel = weaponData.weaponLevel;
        _projectileSpeed = weaponData.projectileSpeed;
        _projectileDamage = weaponData.projectileDamage;
        _projectileNumber = weaponData.projectileNumber;
        _weaponCooldown = weaponData.weaponCooldown;
        _projectileBehaviour = weaponData.projectileBehaviour;

        _playerTransform = playerTransform;

        _projectilesPool = new CustomObjectPool(_projectile, 5);
        _viewportBounds = viewportBounds;

        Debug.Log($"{_viewportBounds.viewportRect.xMin}, {_viewportBounds.viewportRect.yMin}");

        StartCooldown();
    }

    public void Attack()
    {
        if (!_canAttack) return;
        GameObject newProjectile = _projectilesPool.Get();
        newProjectile.transform.position = _playerTransform.position;
        newProjectile.GetComponent<WeaponProjectile>().Initialize( 
            _viewportBounds, 
            ChooseProjectileMovement(newProjectile, _projectileBehaviour), 
            () => _projectilesPool.Release(newProjectile),
            _projectileDamage);
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

    private IProjectileMovement ChooseProjectileMovement(GameObject projectile, ProjectileBehavioursEnum projectileBehaviour)
    {
        switch (projectileBehaviour)
        {
            case ProjectileBehavioursEnum.ProjectileFollowsEnemyMovement:
                return new ProjectileFollowsEnemyMovement(projectile.transform, _projectileSpeed); 
            default:
                Debug.LogError("No movement logic for projectile! Returning basic stuff");
                return new ProjectileFollowsEnemyMovement(projectile.transform, _projectileSpeed);
        }
    }
}
