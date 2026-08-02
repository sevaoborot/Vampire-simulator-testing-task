using UnityEngine;

public class ExpPointSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _expPrefab;
    private CustomObjectPool _expPool;

    public void Initialize()
    {
        _expPool = new CustomObjectPool(_expPrefab, 10);
    }

    public void SpawnExpPoint(Vector2 position)
    {
        GameObject newExpPoint = _expPool.Get();
        newExpPoint.transform.position = position;
        newExpPoint.GetComponent<ExpPoint>().Initialize(() => _expPool.Release(newExpPoint));
    }
}
