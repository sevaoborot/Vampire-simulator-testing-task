using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    public IPlayerWeaponsReader weapons => _playerWeapons;

    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerWeapons _playerWeapons;
    private PlayerLevel _playerLevel;

    public void Initialize(EventBus eventBus, ViewportBounds viewportBounds, InputService inputService)
    {
        _playerHealth = new PlayerHealth(eventBus, _playerData.maxHealth);
        _playerMovement = new PlayerMovement(_playerData.speed, inputService);
        _playerWeapons = new PlayerWeapons(viewportBounds, transform, this, _playerData.weapons[0], eventBus);
        _playerLevel = new PlayerLevel(eventBus, _playerData.expAmountForUnlockingLevel, _playerData.levelIncrement);
    }

    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            _playerMovement?.MovePlayer(transform);
            _playerWeapons?.Attack();
        }            
    }

    //should be replaced with receiving continuous damage in ontriggerstay2d
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (Time.timeScale != 0f)
        {
            if (collision.TryGetComponent<Enemy>(out Enemy currentEnemy))
                _playerHealth?.ReceiveDamage(currentEnemy.Damage);
            if (collision.TryGetComponent<ExpPoint>(out ExpPoint currentExpPoint))
            {
                _playerLevel?.ReceiveExp(currentExpPoint.expAmount);
                currentExpPoint.PickUpExp();
            }
        }
    }

    //on trigger/on collision, where health, exp amount and bonuses
    //_playerHealth.RestoreHealth(amount);
}
