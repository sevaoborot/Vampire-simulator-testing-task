using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : VampSym_Actions.IPlayerActions, IDisposable
{
    private readonly VampSym_Actions _actions;
    private readonly VampSym_Actions.PlayerActions _player;

    public Vector2 MoveDirection { get; private set; }

    public event Action<Vector2> OnMoveChanged;

    public InputService()
    {
        _actions = new VampSym_Actions();
        _player = _actions.Player;
        _player.AddCallbacks(this);
    }

    public void Enable() => _player.Enable();

    public void Disable() => _player.Disable();

    public void Dispose()
    {
        _player.RemoveCallbacks(this);
        _actions.Dispose();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
        OnMoveChanged?.Invoke(MoveDirection);
    }
}