using UnityEngine;


public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private ParticleSystem impactEffect;

    private DamageDealer damageDealer;
    private Vector3 direction;

    public void Initialize(Vector3 direction, DamageDealer dealer)
    {
        damageDealer = dealer;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        // Автоматическое уничтожение через пул
        Invoke("ReturnToPool", lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageDealer != null)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(
                    damageDealer.DamageAmount,
                    damageDealer.DamageType,
                    transform
                );
            }
            PlayImpactEffect();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool("MagicProjectile", gameObject);
    }

    private void PlayImpactEffect()
    {
        if (impactEffect != null)
        {
            ParticleSystem effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}