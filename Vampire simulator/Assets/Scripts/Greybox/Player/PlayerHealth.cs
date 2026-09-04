using System;
using UnityEngine;

public class PlayerHealth
{
    private float _playerCurrentHealth;
    private float _playerMaxHealth;

    private EventBus _eventBus;

    //not sure, but maybe I should make different events on health restored and on damage dealed 
    //public event Action<float, float> OnPlayerHealthChange;

    public float PlayerCurrentHealth 
    {
        get => _playerCurrentHealth;
        private set
        {
            float clamped = Mathf.Clamp(value, 0f, _playerMaxHealth);
            if (Mathf.Approximately(clamped, _playerCurrentHealth)) return; //I remembered there was such a way to compare float values in Unity, but I forgot exact scripting, so I asked Claude about it
            _playerCurrentHealth = clamped;
            if (_playerCurrentHealth <= 0f) _eventBus.Invoke(new DeathSignal());
            else _eventBus.Invoke(new HealthChangedSignal(_playerCurrentHealth));
        }
    }

    public PlayerHealth(EventBus eventBus, float playerMaxHealth)
    {
        _eventBus = eventBus;
        _playerMaxHealth = playerMaxHealth;
        PlayerCurrentHealth = _playerMaxHealth;
    }

    public void ReceiveDamage(float amount)
    {
        PlayerCurrentHealth -= amount;
        Debug.Log($"The player was damaged by the amount of {amount}");
    }

    public void RestoreHealth(float amount)
    {
        PlayerCurrentHealth += amount;
        Debug.Log($"The player was healed by the amount of {amount}");
    }
}
