using System.Collections;
using UnityEngine;

public abstract class Weapon
{
    protected int _currentLevel;
    protected int _maxLevel;
    protected string _weaponID;

    public int CurrentLevel
    {
        get => _currentLevel;
        protected set
        {
            int clamped = Mathf.Clamp(value, 0, _maxLevel);
            _currentLevel = clamped;
        }
    }

    public void UpgrateWeapon()
    {
        CurrentLevel++;
        Debug.Log($"{_weaponID} got level {CurrentLevel}");
    }

    public bool SameID(string ID) => ID == _weaponID;

    public abstract void Attack(MonoBehaviour owner);
}

public class ProjectileWeapon : Weapon
{
    private ProjectileWeaponData _data;

    private Transform _playerTransform;
    private CustomObjectPool _projectilesPool;
    private ViewportBounds _viewportBounds;
    private IPlayerDirectionReader _playerDirection;

    private float _weaponCooldownEndTime;
    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    private float _projectileCooldown = 0.1f;

    public ProjectileWeapon(ProjectileWeaponData weaponData, Transform playerTransform, ViewportBounds viewportBounds, IPlayerDirectionReader playerDirection)
    {
        _data = weaponData;
        _playerTransform = playerTransform;
        _viewportBounds = viewportBounds;
        _playerDirection = playerDirection;

        _weaponID = _data.weaponID; //not cool, should be remade with weaponData class
        CurrentLevel = 0;

        _projectilesPool = new CustomObjectPool(_data.projectile, 5);

        StartCooldown(_data.weaponLevels[0].weaponCooldown);
    }

    public override void Attack(MonoBehaviour owner)
    {
        if (!_canAttack) return;
        //EnableDamageUnit(owner);
        owner.StartCoroutine(CreateProjectile());

    }

    private IEnumerator CreateProjectile() //knife - sometimes projectile get different velocity or speed?
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

    private void StartCooldown(float weaponCooldown) => _weaponCooldownEndTime = Time.time + weaponCooldown;

    private void InterruptCooldown() => _weaponCooldownEndTime = Time.time;

    private IProjectileMovement ChooseProjectileMovement(GameObject projectile, ProjectileBehavioursEnum projectileBehaviour, float projectileSpeed)
    {
        switch (projectileBehaviour)
        {
            case ProjectileBehavioursEnum.Null:
                return null;
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

public class DamagingAreaWeapon : Weapon 
{
    private DamageAreaWeaponData _damageAreaWeaponData;

    private Transform _playerTransform;
    private ViewportBounds _viewportBounds;

    private GameObject _currentArea; 

    public DamagingAreaWeapon(DamageAreaWeaponData data, Transform playerTransform, ViewportBounds viewportBounds)
    {
        _damageAreaWeaponData = data;

        _playerTransform = playerTransform;
        _viewportBounds = viewportBounds;

        _currentArea = CreateArea();
    }

    public override void Attack(MonoBehaviour owner)
    {
        _currentArea.transform.position = _playerTransform.position;
    }

    private GameObject CreateArea()
    {
        float currentAreaSize = _damageAreaWeaponData.weaponLevels[CurrentLevel].areaSize;
        float currentAreaDamage = _damageAreaWeaponData.weaponLevels[CurrentLevel].areaDamage;
        float currentAreasNumber = _damageAreaWeaponData.weaponLevels[CurrentLevel].areasNumber;
        float currentAreaDamageCooldown = _damageAreaWeaponData.weaponLevels[CurrentLevel].areaCooldown;
        float currentAreaExistanceTime = _damageAreaWeaponData.weaponLevels[CurrentLevel].areaExistanceTime;

        GameObject newAreaGameObj = GameObject.Instantiate(_damageAreaWeaponData.projectile, 
            _playerTransform.position, 
            _damageAreaWeaponData.projectile.transform.rotation);
        newAreaGameObj.SetActive(false);
        newAreaGameObj.transform.localScale *= currentAreaSize;

        WeaponDamagingArea newArea = newAreaGameObj.GetComponent<WeaponDamagingArea>();
        newArea.Initialize(currentAreaDamage, currentAreaExistanceTime, currentAreaDamageCooldown);

        newAreaGameObj.SetActive(true);
        return newAreaGameObj;
    }
}