using UnityEngine;

public class PlayerMovement 
{
    private float _speed;
    private InputService _inputService;

    public PlayerMovement(float speed, InputService inputService)
    {
        _speed = speed;
        _inputService = inputService;
    }

    public void MovePlayer(Transform player)
    {
        if (_inputService == null)
        {
            Debug.LogError("Input Service is not initialized in Player Movement!");
            return;
        } 

        Vector2 movementVector = _inputService.MoveDirection * _speed * Time.deltaTime;
        player.position = (Vector2)player.position + movementVector;
    }
}
