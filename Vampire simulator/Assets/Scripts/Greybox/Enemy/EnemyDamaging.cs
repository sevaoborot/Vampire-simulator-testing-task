using UnityEngine;

public class EnemyDamaging : MonoBehaviour
{
    [SerializeField] private float _enemyDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Damaging smth");
        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)) playerHealth.OnDamageDealed(_enemyDamage);
    }
}
