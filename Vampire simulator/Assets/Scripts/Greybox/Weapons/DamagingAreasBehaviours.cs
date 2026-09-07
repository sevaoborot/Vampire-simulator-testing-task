using UnityEngine;

#region movement

public interface IDamagingAreaMovement
{
    public Vector2 Move();
}

public enum DamagingAreaMovementsEnum
{
    Null,
    DamagingAreaFollowsThePlayer
}

public class DamagingAreaFollowsThePlayerBehaviour : IDamagingAreaMovement
{
    private Transform _player;

    public DamagingAreaFollowsThePlayerBehaviour(Transform ownerTransform)
    {
        _player = ownerTransform;
    }

    public Vector2 Move() => _player.position;
}

#endregion

#region spawning

public interface IDamagingAreaSpawning
{
    public Vector2 Spawn();
}

public enum DamagingAreaSpawningEnum
{
    RandomPositionSpawning,
    SpawningAtPlayerPosition
}

public class RandomPositionSpawning : IDamagingAreaSpawning
{
    private ViewportBounds _viewportBounds;

    public RandomPositionSpawning(ViewportBounds viewportBounds)
    {
        _viewportBounds = viewportBounds;
    }

    public Vector2 Spawn()
    {
        _viewportBounds.UpdateViewportBoundsPosition();
        return GetRandomPositionToSpawn();
    }

    private Vector2 GetRandomPositionToSpawn()
    {
        float spawnXmin = _viewportBounds.viewportRect.xMin + _viewportBounds.viewportRect.width * 0.1f;
        float spawnXmax = _viewportBounds.viewportRect.xMax - _viewportBounds.viewportRect.width * 0.1f;
        float spawnYmin = _viewportBounds.viewportRect.yMin + _viewportBounds.viewportRect.height * 0.1f;
        float spawnYmax = _viewportBounds.viewportRect.yMax - _viewportBounds.viewportRect.height * 0.1f;

        return new Vector2(
            Random.Range(spawnXmin, spawnXmax),
            Random.Range(spawnYmin, spawnYmax));
    }
}

public class SpawningAtPlayerPosition : IDamagingAreaSpawning
{
    private Transform _player;

    public SpawningAtPlayerPosition(Transform player)
    {
        _player = player;
    }

    public Vector2 Spawn() => _player.position;
}

#endregion