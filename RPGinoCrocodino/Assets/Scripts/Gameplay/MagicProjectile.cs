using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private ParticleSystem impactEffect;

    private DamageDealer damageDealer;
    private Vector3 direction;

    public void Initialize(Vector3 targetDirection, DamageDealer dealer)
    {
        direction = targetDirection.normalized;
        damageDealer = dealer;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            damageDealer.HandleImpact(other);
            PlayImpactEffect();
            Destroy(gameObject);
        }
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