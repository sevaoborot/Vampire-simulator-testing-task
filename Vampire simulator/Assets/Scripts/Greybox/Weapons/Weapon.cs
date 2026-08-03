using UnityEngine;

public class Weapon 
{
    private WeaponData _data;

    private int _currentLevel;

    public int CurrentLevel
    {
        get => _currentLevel;
        private set
        {
            int clamped = Mathf.Clamp(value, 0, _data.weaponLevels.Length - 1);
            _currentLevel = clamped;
        }
    }

    private Transform _playerTransform;
    private CustomObjectPool _projectilesPool;
    private float _weaponCooldownEndTime;
    private ViewportBounds _viewportBounds;

    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    public Weapon(WeaponData weaponData, Transform playerTransform, ViewportBounds viewportBounds)
    {
        _data = weaponData;
        CurrentLevel = 0;
        _playerTransform = playerTransform;

        _projectilesPool = new CustomObjectPool(_data.projectile, 5);
        _viewportBounds = viewportBounds;

        StartCooldown(_data.weaponLevels[0].weaponCooldown);
    }

    public void Attack()
    {
        if (!_canAttack) return;

        float currentSpeed = _data.weaponLevels[CurrentLevel].projectileSpeed;
        float currentDamage = _data.weaponLevels[CurrentLevel].projectileDamage;
        int currentNumber = _data.weaponLevels[CurrentLevel].projectileNumber;
        float currentCooldown = _data.weaponLevels[CurrentLevel].weaponCooldown;

        GameObject newPorjectile = _projectilesPool.Get();
        newPorjectile.transform.position = _playerTransform.position;
        newPorjectile.GetComponent<WeaponProjectile>().Initialize(
            _viewportBounds,
            ChooseProjectileMovement(newPorjectile, _data.projectileBehaviour, currentSpeed),
            () => _projectilesPool.Release(newPorjectile),
            currentDamage);
        StartCooldown(currentCooldown);

        //stop cooldowns
    }

    public void UpgrateWeapon() => CurrentLevel++;

    public bool SameID(string ID) => ID == _data.weaponID;

    private void StartCooldown(float weaponCooldown)
    {
        _weaponCooldownEndTime = Time.time + weaponCooldown;
    }

    private void InterruptCooldown()
    {
        _weaponCooldownEndTime = Time.time;
    }

    private IProjectileMovement ChooseProjectileMovement(GameObject projectile, ProjectileBehavioursEnum projectileBehaviour, float projectileSpeed)
    {
        switch (projectileBehaviour)
        {
            case ProjectileBehavioursEnum.ProjectileFollowsEnemyMovement:
                return new ProjectileFollowsEnemyMovement(projectile.transform, projectileSpeed); 
            default:
                Debug.LogError("No movement logic for projectile! Returning basic stuff");
                return new ProjectileFollowsEnemyMovement(projectile.transform, projectileSpeed);
        }
    }
}
