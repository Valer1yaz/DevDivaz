using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    private int damage;

    public void SetDamage(int value) => damage = value;

    private void Start() => Destroy(gameObject, lifetime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage, DamageType.Magic);
            Destroy(gameObject);
        }
    }
}