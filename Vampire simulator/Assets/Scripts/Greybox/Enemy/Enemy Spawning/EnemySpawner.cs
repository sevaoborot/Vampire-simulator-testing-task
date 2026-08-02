using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExpPointSpawner))]
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn area settings")]
    [SerializeField] private float _spawnOffset;
    private EnemySpawnArea _spawnArea;

    [Header("Enemies settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawningCooldown;
    [SerializeField] private Transform _player;

    private CustomObjectPool _enemyPool;
    private ExpPointSpawner _expPointSpawner;
    private float _enemySpawningCooldownEndTime;

    private bool _canSpawn => Time.time >= _enemySpawningCooldownEndTime;

    public void Initialize(ViewportBounds viewportBounds)
    {
        _spawnArea = new EnemySpawnArea(viewportBounds, _spawnOffset);
        _enemyPool = new CustomObjectPool(_enemyPrefab, 10);
        _expPointSpawner = GetComponent<ExpPointSpawner>();
        _expPointSpawner.Initialize();
    }

    private void Update()
    {
        SpawnEnemy();
    }

    private void LateUpdate()
    {
        _spawnArea.EnemySpawnAreaUpdate();
    }

    private void SpawnEnemy()
    {
        if (!_canSpawn) return;
        GameObject newEnemy = _enemyPool.Get();
        newEnemy.transform.position = _spawnArea.GetRandomPositionToSpawn();
        newEnemy.transform.SetParent(transform);

        Enemy enemy = newEnemy.GetComponent<Enemy>();
        EnemyRegistry.Register(enemy);
        enemy.Initialize(_player, () =>
        {
            EnemyRegistry.Unregister(enemy);
            _expPointSpawner.SpawnExpPoint(newEnemy.transform.position);
            _enemyPool.Release(newEnemy);
        });
        StartSpawnCooldown();

        //stop cooldown        
    }

    private void StartSpawnCooldown()
    {
        _enemySpawningCooldownEndTime = Time.time + _spawningCooldown;
    }

    private void InterruptCooldown()
    {
        _enemySpawningCooldownEndTime = Time.time;
    }
}

public static class EnemyRegistry
{
    private static readonly List<Enemy> _activeEnemies = new List<Enemy>(128);

    public static IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies;

    public static void Register(Enemy enemy)
    {
        if (!_activeEnemies.Contains(enemy)) _activeEnemies.Add(enemy);
    }

    public static void Unregister(Enemy enemy) => _activeEnemies.Remove(enemy);
}