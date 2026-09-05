using UnityEngine;

public interface IDamagingAreaMovement
{
    public Vector2 Move();
}

public enum DamagingAreaMovementsEnum
{
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