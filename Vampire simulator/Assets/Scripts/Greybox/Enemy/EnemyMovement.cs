using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector2 _enemyVelocity;
    [SerializeField] private float _enemyMaxSpeed;

    private void Update()
    {
        transform.position = Vector2.SmoothDamp((Vector2)transform.position, _player.position, ref _enemyVelocity, 0, _enemyMaxSpeed, Time.deltaTime);
    }
}
