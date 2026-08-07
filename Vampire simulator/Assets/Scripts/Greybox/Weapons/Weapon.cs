using System.Collections;
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
    private ViewportBounds _viewportBounds;
    private IPlayerDirectionReader _playerDirection;

    private float _weaponCooldownEndTime;
    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    private float _projectileCooldown = 0.1f; 

    public Weapon(WeaponData weaponData, Transform playerTransform, ViewportBounds viewportBounds, IPlayerDirectionReader playerDirection)
    {
        _data = weaponData;
        _playerTransform = playerTransform;
        _viewportBounds = viewportBounds;
        _playerDirection = playerDirection;

        CurrentLevel = 0;

        _projectilesPool = new CustomObjectPool(_data.projectile, 5);

        StartCooldown(_data.weaponLevels[0].weaponCooldown);
    }

    public void Attack(MonoBehaviour owner)
    {
        if (!_canAttack) return;
        owner.StartCoroutine(CreateProjectile()); 
    }

    private IEnumerator CreateProjectile() 
    {
        float currentSpeed = _data.weaponLevels[CurrentLevel].projectileSpeed;
        float currentDamage = _data.weaponLevels[CurrentLevel].projectileDamage;
        int currentProjectileNumber = _data.weaponLevels[CurrentLevel].projectileNumber;
        float currentCooldown = _data.weaponLevels[CurrentLevel].weaponCooldown;
        int currentMaxEnemiesToHit = _data.weaponLevels[CurrentLevel].maxEnemiesToHit;

        Debug.Log($"current number of projectiles: {currentProjectileNumber}");

        StartCooldown(currentCooldown);

        for (int i = 0; i < currentProjectileNumber; i++)
        {
            GameObject newPorjectile = _projectilesPool.Get();
            newPorjectile.transform.position = _playerTransform.position;
            newPorjectile.GetComponent<WeaponProjectile>().Initialize( // error 
                _viewportBounds,
                ChooseProjectileMovement(newPorjectile, _data.projectileBehaviour, currentSpeed),
                () => _projectilesPool.Release(newPorjectile),
                currentDamage,
                currentMaxEnemiesToHit);

            yield return new WaitForSeconds(_projectileCooldown);
        }
    }

    public void UpgrateWeapon()
    {
        CurrentLevel++;
        Debug.Log($"{_data.weaponID} got level {CurrentLevel}");
    }

    public bool SameID(string ID) => ID == _data.weaponID;

    private void StartCooldown(float weaponCooldown) => _weaponCooldownEndTime = Time.time + weaponCooldown;

    private void InterruptCooldown() => _weaponCooldownEndTime = Time.time;

    private IProjectileMovement ChooseProjectileMovement(GameObject projectile, ProjectileBehavioursEnum projectileBehaviour, float projectileSpeed)
    {
        switch (projectileBehaviour)
        {
            case ProjectileBehavioursEnum.ProjectileFollowsEnemyMovement:
                return new ProjectileFollowsEnemyMovement(projectile.transform, projectileSpeed);
            case ProjectileBehavioursEnum.PlayerFacedDirectionProjectileMovement:
                return new PlayerFacedDirectionProjectileMovement(_playerDirection.LastDirection, projectileSpeed); 
            default:
                Debug.LogError("No movement logic for projectile! Returning basic stuff");
                return new ProjectileFollowsEnemyMovement(projectile.transform, projectileSpeed);
        }
    }
}
