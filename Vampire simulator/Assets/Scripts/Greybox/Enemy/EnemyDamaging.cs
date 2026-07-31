using UnityEngine;

public class EnemyDamaging : MonoBehaviour
{
    [SerializeField] private float _enemyDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            Debug.Log("Damaging smth");
            playerHealth.OnDamageDealed(_enemyDamage);
        }
    }
}
