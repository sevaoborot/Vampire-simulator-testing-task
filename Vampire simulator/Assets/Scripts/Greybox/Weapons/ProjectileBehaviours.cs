using UnityEngine;

public interface IProjectileMovement
{
    public Vector2 Move(Transform projectileTransform);
}

public enum ProjectileBehavioursEnum
{
    Null, //should I really keep it or can I remove it?
    ProjectileFollowsEnemyMovement,
    PlayerFacedDirectionProjectileMovement
}

public class ProjectileFollowsEnemyMovement : IProjectileMovement
{
    private Transform _player;
    private float _speed;

    private Vector2 _movementVector;

    public ProjectileFollowsEnemyMovement(Transform player, float speed)
    {
        _player = player;
        _speed = speed;

        
        Vector2 movementVectorUnnormalized = FindNearestEnemy() - (Vector2)_player.position;
        _movementVector = movementVectorUnnormalized.normalized * _speed * Time.deltaTime;
    }

    public Vector2 Move(Transform projectileTransform)
    {
        projectileTransform.position = (Vector2)projectileTransform.position + _movementVector;
        return _movementVector;
    }

    private Vector2 FindNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (Enemy enemy in EnemyRegistry.ActiveEnemies)
        {
            float sqrDistance = (enemy.transform.position - _player.position).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                nearestEnemy = enemy;
                nearestSqrDistance = sqrDistance;
            }
        }

        return (Vector2)nearestEnemy.transform.position;
    }
}

public class PlayerFacedDirectionProjectileMovement : IProjectileMovement
{
    private float _speed;
    private Vector2 _direction;
    private Vector2 _movementVector;

    public PlayerFacedDirectionProjectileMovement(Vector2 playerDirection, float speed)
    {
        Debug.Log(playerDirection);
        _direction = playerDirection;
        _speed = speed;;

        _movementVector = _direction.normalized * _speed * Time.deltaTime;

    }

    public Vector2 Move(Transform projectileTransform)
    {
        projectileTransform.position = (Vector2)projectileTransform.position + _movementVector;
        return _movementVector;
    }
}
