using UnityEngine;

public interface IProjectileMovement
{
    public void Move(Transform projectileTransform);
}

public enum ProjectileBehavioursEnum
{
    ProjectileFollowsEnemyMovement
}

public class ProjectileFollowsEnemyMovement : IProjectileMovement
{
    public ProjectileFollowsEnemyMovement(Transform owner, float speed)
    {
        //constructor
    }

    public void Move(Transform projectileTransform)
    {
        //logic for the movement
    }
}
