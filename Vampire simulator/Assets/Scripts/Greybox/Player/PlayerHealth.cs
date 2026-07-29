using System;
using UnityEngine;

public class PlayerHealth: MonoBehaviour 
{
    [SerializeField] private float _playerCurrentHealth;
    [SerializeField] private float _playerMaxHealth;

    //not sure, but maybe I should make different events on health restored and on damage dealed 
    public event Action<float, float> OnPlayerHealthChange;

    private float PlayerCurrentHealth //like a protection from fool???
    {
        get => _playerCurrentHealth;
        set
        {
            float clamped = Mathf.Clamp(value, 0f, _playerMaxHealth);
            //if (clamped == _playerCurrentHealth) return;
            if (Mathf.Approximately(clamped, _playerCurrentHealth)) return; //I remembered there was such a way to compare float values in Unity, but I forgot exact scripting, so I asked Claude about it
            _playerCurrentHealth = clamped;
            OnPlayerHealthChange?.Invoke(_playerCurrentHealth, _playerMaxHealth);
            if (_playerCurrentHealth <= 0f) Debug.LogWarning("The player is dead!");
        }
    }

    public void OnDamageDealed(float amount)
    {
        PlayerCurrentHealth -= amount;
        Debug.Log($"The player was damaged by the amount of {amount}");
    }

    public void OnHealthRestored(float amount)
    {
        PlayerCurrentHealth += amount;
        Debug.Log($"The player was healed by the amount of {amount}");
    }
}
