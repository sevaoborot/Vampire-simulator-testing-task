using UnityEngine;

public interface IPlayerDirectionReader
{
    public Vector2 LastDirection { get; }
}

public class PlayerMovement : IPlayerDirectionReader
{
    private float _speed;
    private InputService _inputService;
    private Vector2 _lastDirection;

    public Vector2 LastDirection => _lastDirection;

    public PlayerMovement(float speed, InputService inputService)
    {
        _speed = speed;
        _inputService = inputService;
        _lastDirection = Vector2.up;
    }

    public void MovePlayer(Transform player)
    {
        if (_inputService == null)
        {
            Debug.LogError("Input Service is not initialized in Player Movement!");
            return;
        }

        if (_inputService.MoveDirection != Vector2.zero) _lastDirection = _inputService.MoveDirection;
        Vector2 movementVector = _inputService.MoveDirection * _speed * Time.deltaTime;
        player.position = (Vector2)player.position + movementVector;
    }
}
