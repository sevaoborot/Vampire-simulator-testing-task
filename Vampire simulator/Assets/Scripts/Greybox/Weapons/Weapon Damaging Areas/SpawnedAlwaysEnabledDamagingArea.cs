using UnityEngine;

public class SpawnedAlwaysEnabledDamagingArea : SpawnedDamagingArea
{
    public void Initialize(IDamagingAreaMovement areaMovement, float areaDamage, float areaDamageCooldown) => 
        ProtectedInitialize(areaMovement, areaDamage, areaDamageCooldown);

    public void Upgrate(float areaDamage, float areaDamageCooldown, float areaSize) 
    {
        Damage = areaDamage;
        _areaDamageCooldown = areaDamageCooldown;

        gameObject.transform.localScale = new Vector2(areaSize, areaSize);
    }
}
