using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerWeapons))]
public class PlayerData : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerWeapons _playerWeapons;

    public void Initialize(ViewportBounds viewportBounds)
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerWeapons = GetComponent<PlayerWeapons>();

        //_playerHealth.Initialize();
        _playerMovement.Initialize();
        _playerWeapons.Initialize(viewportBounds);
    }
}
