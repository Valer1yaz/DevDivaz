using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    private int damage;
    private DamageType damageType;
    private Vector3 targetPosition;
    private float speed;

    public void Initialize(int dmg, DamageType type, float spd, Vector3 target)
    {
        damage = dmg;
        damageType = type;
        speed = spd;
        targetPosition = target;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        if (impactEffect != null)
        {
            var effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }
        // Автоуничтожение при промахе
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            Destroy(gameObject, 5f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage, damageType);
            Destroy(gameObject);
        }
    }
}