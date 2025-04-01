using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount, DamageType damageType, Transform damageSource = null);
}