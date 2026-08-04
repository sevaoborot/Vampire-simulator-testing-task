using UnityEngine;

public class GamemodeInitialize : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private UI _ui;
    [SerializeField] private Camera _camera;

    private EventBus _eventBus;
    private ViewportBounds _viewportBounds;
    private InputService _inputService;

    private void Awake()
    {
        _eventBus = new EventBus();

        _viewportBounds = new ViewportBounds(_camera);
        _inputService = new InputService();
        _inputService.Enable();

        _player.Initialize(_eventBus, _viewportBounds, _inputService);
        _enemySpawner.Initialize(_viewportBounds);
        _ui.Initialize(_eventBus, _player.weapons);
    }

    private void OnEnable()
    {
        _inputService?.Enable();
    }

    private void OnDisable()
    {
        _inputService?.Disable();
    }

    private void OnDestroy()
    {
        _inputService?.Dispose();
    }
}
