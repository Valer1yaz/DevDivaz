public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private DamageType damageType;
    [SerializeField] private string[] targetTags = { "Enemy", "Player" };

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageAmount, damageType);

                    // ���� ���������� ������, ���������� ��� ����� ������������
                    if (GetComponent<Projectile>() != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}