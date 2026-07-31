using UnityEngine;

public class EnemyMovement 
{
    private Vector2 _velocity;

    public Vector2 Move(Transform enemy, Transform player, float maxSpeed)
    {
        return Vector2.SmoothDamp(
            (Vector2)enemy.position,
            player.position,
            ref _velocity,
            0,
            maxSpeed,
            Time.deltaTime);
    }
}
