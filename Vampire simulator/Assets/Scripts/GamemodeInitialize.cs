using UnityEngine;

public class GamemodeInitialize : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Camera _camera;

    private ViewportBounds _viewportBounds;

    private void Awake()
    {
        _viewportBounds = new ViewportBounds(_camera);

        _playerData.Initialize(_viewportBounds);
        _enemySpawner.Initialize(_viewportBounds); 
        
    }
}
