using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Vampire Sim/New enemy")]
public class EnemyData : ScriptableObject
{
    public float Damage;
    public float MaxHealth;
    public float MaxSpeed;
    public float ExpPoint;
}
