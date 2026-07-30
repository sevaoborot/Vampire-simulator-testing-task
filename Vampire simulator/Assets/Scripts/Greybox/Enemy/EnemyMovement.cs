using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform _player;
    private Vector2 _enemyVelocity;
    [SerializeField] private float _enemyMaxSpeed;

    public Transform Player
    {
        private get => _player;
        set
        {
            if (_player == null) _player = value;
        }
    }

    private void Update()
    {
        transform.position = Vector2.SmoothDamp((Vector2)transform.position, _player.position, ref _enemyVelocity, 0, _enemyMaxSpeed, Time.deltaTime);
    }
}
