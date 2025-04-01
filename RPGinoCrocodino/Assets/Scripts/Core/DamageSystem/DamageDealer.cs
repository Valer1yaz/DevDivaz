using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private DamageType damageType;
    [SerializeField] private string[] targetTags = { "Enemy", "Player" };

    public int DamageAmount => damageAmount;
    public DamageType DamageType => damageType;

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    // Теперь передаем три параметра
                    damageable.TakeDamage(damageAmount, damageType, transform);

                    if (TryGetComponent<IProjectile>(out _))
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}