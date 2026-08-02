using System;
using UnityEngine;

public class PlayerLevel 
{
    //initialzed
    private readonly float _expAmountForUnlockingLevel; //cant be negative
    private readonly float _levelIncrement; //mb short instead

    //proceed
    private int _currentLevel; //mb short instead; cant be less than 1
    private float _currentLevelExpAmount;
    private float _expAmountForUnlockingNextLevel;
    private float _totalExpAmount; //cant be negative

    private EventBus _eventBus;

    public int CurrentLevel
    {
        get => _currentLevel;
        private set
        {
            if (value < 0) return;
            _currentLevel = value;
            _expAmountForUnlockingNextLevel *= _levelIncrement;
            CurrentLevelExpAmount = 0f;
            _eventBus.Invoke(new LevelChangedSignal(_currentLevel));
        }
    }

    public float CurrentLevelExpAmount //I think, I need to rewrite it 
    {
        get => _currentLevelExpAmount;
        private set
        {
            float clamped = Mathf.Clamp(value, 0f, _expAmountForUnlockingNextLevel);
            if (Mathf.Approximately(clamped, _currentLevelExpAmount)) return;
            _currentLevelExpAmount = clamped;
            _eventBus.Invoke(new CurrentLevelExpChangedSignal(_currentLevelExpAmount));
            if (_currentLevelExpAmount >= _expAmountForUnlockingNextLevel) CurrentLevel++;
        }
    }

    public float TotalExpAmount
    {
        get => _totalExpAmount;
        private set
        {
            if (value < _totalExpAmount) return; //should always increase
            _totalExpAmount = value;
        }
    }

    public PlayerLevel(EventBus eventBus, float expAmountForUnlockingLevel, float levelIncrement)
    {
        _eventBus = eventBus;
        CurrentLevel = 0;
        _expAmountForUnlockingLevel = expAmountForUnlockingLevel;
        _levelIncrement = levelIncrement;
        _expAmountForUnlockingNextLevel = _expAmountForUnlockingLevel * _levelIncrement;
    }

    public void ReceiveExp(float amount)
    {
        TotalExpAmount += amount;
        CurrentLevelExpAmount += amount;
    }
}
