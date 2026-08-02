using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerWeapons _playerWeapons;
    private PlayerLevel _playerLevel;

    public void Initialize(ViewportBounds viewportBounds, InputService inputService)
    {
        _playerHealth = new PlayerHealth(_playerData.maxHealth);
        _playerMovement = new PlayerMovement(_playerData.speed, inputService);
        _playerWeapons = new PlayerWeapons(viewportBounds, transform, _playerData.weapons[0]);
        _playerLevel = new PlayerLevel(_playerData.expAmountForUnlockingLevel, _playerData.levelIncrement);
    }

    private void Update()
    {
        _playerMovement?.MovePlayer(transform);
        _playerWeapons?.Attack();
    }

    //should be replaced with receiving continuous damage in ontriggerstay2d
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.TryGetComponent<Enemy>(out Enemy currentEnemy))
            _playerHealth?.ReceiveDamage(currentEnemy.Damage);
        if (collision.TryGetComponent<ExpPoint>(out ExpPoint currentExpPoint))
        {
            _playerLevel?.ReceiveExp(currentExpPoint.expAmount);
            currentExpPoint.PickUpExp();
        }
    }

    //on trigger/on collision, where health, exp amount and bonuses
    //_playerHealth.RestoreHealth(amount);
}
