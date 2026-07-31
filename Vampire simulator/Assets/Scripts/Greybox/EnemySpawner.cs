using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn area settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _spawnOffset;
    private EnemySpawnArea _spawnArea;

    [Header("Enemies settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawningCooldown;
    [SerializeField] private Transform _player;
    private CustomObjectPool _enemyPool;
    private float _enemySpawningCooldownEndTime;

    private bool _canSpawn => Time.time >= _enemySpawningCooldownEndTime;

    private void Awake()
    {
        _spawnArea = new EnemySpawnArea(_camera, _spawnOffset);
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
        newEnemy.GetComponent<EnemyMovement>().Player = _player;
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
