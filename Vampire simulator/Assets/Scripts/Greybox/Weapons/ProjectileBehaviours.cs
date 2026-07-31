using UnityEngine;

public interface IProjectileMovement
{
    public Vector2 Move(Transform projectileTransform);
}

public enum ProjectileBehavioursEnum
{
    ProjectileFollowsEnemyMovement
}

public class ProjectileFollowsEnemyMovement : IProjectileMovement
{
    private Transform _owner;
    private float _speed;

    private EnemyMovement _enemyMovement; //EnemyMovement should be replaced later with some Enemy Data
    private Vector2 _movementVector;

    public ProjectileFollowsEnemyMovement(Transform owner, float speed)
    {

        _owner = owner;
        _speed = speed;

        
        Vector2 movementVectorUnnormalized = FindNearestEnemy() - (Vector2)_owner.position;
        _movementVector = movementVectorUnnormalized.normalized * _speed * Time.deltaTime;
    }

    public Vector2 Move(Transform projectileTransform)
    {
        _owner.position = (Vector2)_owner.position + _movementVector;
        return _movementVector;
    }

    private Vector2 FindNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (Enemy enemy in EnemyRegistry.ActiveEnemies)
        {
            float sqrDistance = (enemy.transform.position - _owner.position).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                nearestEnemy = enemy;
                nearestSqrDistance = sqrDistance;
            }
        }

        return (Vector2)nearestEnemy.transform.position;
    }
}
