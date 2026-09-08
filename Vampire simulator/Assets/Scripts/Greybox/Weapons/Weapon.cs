using System.Collections;
using UnityEngine;

public abstract class Weapon
{
    protected int _currentLevel = 0;
    protected int _maxLevel;
    protected string _weaponID;

    public int CurrentLevel
    {
        get => _currentLevel;
        protected set
        {
            //int clamped = Mathf.Clamp(value, 0, _maxLevel);
            if (value >= 0 || value <= _maxLevel) _currentLevel = value;
        }
    }

    public virtual void UpgrateWeapon()
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
        //CurrentLevel = 0;

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

        StartCooldown(currentCooldown);

        for (int i = 0; i < currentProjectileNumber; i++)
        {
            GameObject newPorjectile = _projectilesPool.Get();
            newPorjectile.transform.position = _playerTransform.position;
            newPorjectile.GetComponent<WeaponProjectile>().Initialize( 
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
            //case ProjectileBehavioursEnum.Null:
            //    return null;
            case ProjectileBehavioursEnum.ProjectileFollowsEnemyMovement:
                return new ProjectileFollowsEnemyMovement(projectile.transform, projectileSpeed);
            case ProjectileBehavioursEnum.PlayerFacedDirectionProjectileMovement:
                return new PlayerFacedDirectionProjectileMovement(_playerDirection.LastDirection, projectileSpeed);
            default:
                Debug.LogWarning("No movement logic for projectile!");
                return null;
        }
    }
}

public abstract class DamagingAreaWeapon : Weapon //should I make in
{
    protected Transform _playerTransform;
    private ViewportBounds _viewportBounds;

    public DamagingAreaWeapon(Transform playerTransform, ViewportBounds viewportBounds)
    {
        _playerTransform = playerTransform;
        _viewportBounds = viewportBounds;
    }

    protected IDamagingAreaMovement ChooseMovement(DamagingAreaMovementsEnum areaMovement)
    {
        switch (areaMovement)
        {
            case DamagingAreaMovementsEnum.Null:
                return null;
            case DamagingAreaMovementsEnum.DamagingAreaFollowsThePlayer:
                return new DamagingAreaFollowsThePlayerBehaviour(_playerTransform);
            default:
                Debug.LogWarning("No movement type selected for this type of area!");
                return null;
        }
    }

    protected IDamagingAreaSpawning ChooseSpawning(DamagingAreaSpawningEnum spawning) { 
        switch (spawning)
        {
            case DamagingAreaSpawningEnum.SpawningAtPlayerPosition:
                return new SpawningAtPlayerPosition(_playerTransform);
            case DamagingAreaSpawningEnum.RandomPositionSpawning:
                return new RandomPositionSpawning(_viewportBounds);
            default:
                Debug.LogError("No spawning behaviour selected for this typ of area!");
                return new SpawningAtPlayerPosition(_playerTransform);
        }
    }
}

public class AlwaysEnabledDamagingAreaWeapon : DamagingAreaWeapon
{
    private AlwaysEnabledDamagingAreaWeaponData _data;
    private SpawnedAlwaysEnabledDamagingArea _currentArea;

    public AlwaysEnabledDamagingAreaWeapon(AlwaysEnabledDamagingAreaWeaponData data, Transform playerTransform, ViewportBounds viewportBounds) : base(playerTransform, viewportBounds)
    {
        _data = data;
        _weaponID = _data.weaponID;

        _currentArea = CreateArea();
    }

    public override void UpgrateWeapon()
    {
        base.UpgrateWeapon();
        _currentArea.Upgrate(
            _data.weaponLevels[CurrentLevel].areaDamage,
            _data.weaponLevels[CurrentLevel].areaDamageCooldown,
            _data.weaponLevels[CurrentLevel].areaSize);
    }

    public override void Attack(MonoBehaviour owner)
    {
        
    }

    private SpawnedAlwaysEnabledDamagingArea CreateArea()
    {
        float currentAreaSize = _data.weaponLevels[CurrentLevel].areaSize;
        float currentAreaDamage = _data.weaponLevels[CurrentLevel].areaDamage;
        float currentAreaDamageCooldown = _data.weaponLevels[CurrentLevel].areaDamageCooldown;

        GameObject newAreaGameObj = GameObject.Instantiate(_data.projectile,
            _playerTransform.position,
            _data.projectile.transform.rotation);
        newAreaGameObj.SetActive(false);
        newAreaGameObj.transform.localScale *= currentAreaSize;

        SpawnedAlwaysEnabledDamagingArea newArea = newAreaGameObj.GetComponent<SpawnedAlwaysEnabledDamagingArea>();

        newArea.Initialize(
            ChooseMovement(_data.areaMovementType),
            currentAreaDamage,
            currentAreaDamageCooldown);

        newAreaGameObj.SetActive(true);
        return newArea;
    }
}

public class RegularDamagingAreaWeapon : DamagingAreaWeapon
{
    private RegularDamagingAreaWeaponData _data;
    private CustomObjectPool _areasPool;

    private float _weaponCooldownEndTime;
    private bool _canAttack => Time.time >= _weaponCooldownEndTime;

    public RegularDamagingAreaWeapon(RegularDamagingAreaWeaponData data, Transform playerTransform, ViewportBounds viewportBounds) : base(playerTransform, viewportBounds)
    {
        _data = data;
        _weaponID = _data.weaponID;

        _areasPool = new CustomObjectPool(_data.projectile, 4);

        StartCooldown(_data.weaponLevels[CurrentLevel].areaSpawningCooldown);
    }

    public override void UpgrateWeapon()
    {
        base.UpgrateWeapon();
    }

    public override void Attack(MonoBehaviour owner)
    {
        if (!_canAttack) return;
        CreateArea();
    }

    private void StartCooldown(float weaponCooldown) => _weaponCooldownEndTime = Time.time + weaponCooldown;

    private void CreateArea()
    {
        float currentAreaSize = _data.weaponLevels[CurrentLevel].areaSize;
        float currentAreaDamage = _data.weaponLevels[CurrentLevel].areaDamage;
        float currentAreasNumber = _data.weaponLevels[CurrentLevel].areasNumber;
        float currentAreaDamageCooldown = _data.weaponLevels[CurrentLevel].areaDamageCooldown;
        float currentAreaExistanceTime = _data.weaponLevels[CurrentLevel].areaExistanceTime;

        for (int i = 0; i < currentAreasNumber; i++)
        {
            GameObject newArea = _areasPool.Get();
            newArea.transform.localScale = new Vector2(currentAreaSize, currentAreaSize);
            newArea.transform.position = ChooseSpawning(_data.areaSpawning).Spawn();
            newArea.GetComponent<SpawnedRegularDamagingArea>().Initialize(
                ChooseMovement(_data.areaMovementType),
                currentAreaDamage,
                currentAreaExistanceTime,
                currentAreaDamageCooldown,
                () => _areasPool.Release(newArea));
        }

        StartCooldown(currentAreaExistanceTime + _data.weaponLevels[CurrentLevel].areaSpawningCooldown);
    }
}