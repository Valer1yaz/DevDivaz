using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 3f;
    private DamageDealer damageDealer;

    public void Initialize(DamageDealer dealer)
    {
        damageDealer = dealer;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageDealer != null && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(
                    damageDealer.DamageAmount,
                    damageDealer.DamageType,
                    transform
                );
            }
            Destroy(gameObject);
        }
    }
}