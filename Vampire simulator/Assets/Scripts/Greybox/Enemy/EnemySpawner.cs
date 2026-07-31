using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn area settings")]
    //[SerializeField] private Camera _camera;
    [SerializeField] private float _spawnOffset;
    private EnemySpawnArea _spawnArea;

    [Header("Enemies settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawningCooldown;
    [SerializeField] private Transform _player;
    private CustomObjectPool _enemyPool;
    private float _enemySpawningCooldownEndTime;

    private bool _canSpawn => Time.time >= _enemySpawningCooldownEndTime;

    public void Initialize(ViewportBounds viewportBounds)
    {
        _spawnArea = new EnemySpawnArea(viewportBounds, _spawnOffset);
        _enemyPool = new CustomObjectPool(_enemyPrefab, 10);
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

        //EnemyMovement and EnemyHealth should be replaced later with some Enemy Data
        EnemyMovement newEnemyMovement = newEnemy.GetComponent<EnemyMovement>();
        newEnemyMovement.Player = _player; 
        EnemyRegistry.Register(newEnemyMovement);
        newEnemy.GetComponent<EnemyHealth>().Initialize(() => _enemyPool.Release(newEnemy));
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
    private static readonly List<EnemyMovement> _activeEnemies = new List<EnemyMovement>(128);

    public static IReadOnlyList<EnemyMovement> ActiveEnemies => _activeEnemies;

    public static void Register(EnemyMovement enemy)
    {
        if (!_activeEnemies.Contains(enemy)) _activeEnemies.Add(enemy);
    }

    public static void Unregister(EnemyMovement enemy) => _activeEnemies.Remove(enemy);
}