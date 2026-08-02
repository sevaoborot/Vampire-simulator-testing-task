using System;
using UnityEngine;

public class ExpPoint : MonoBehaviour
{
    [SerializeField] private float _expAmount;
    public float expAmount => _expAmount;

    private Action _OnPickUp;

    public void Initialize(Action onPickUp)
    {
        _OnPickUp = onPickUp;
    }

    public void PickUpExp()
    {
        _OnPickUp();
    }
}
