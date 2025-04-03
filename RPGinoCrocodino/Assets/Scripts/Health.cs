using UnityEngine;

public enum DamageType { Physical, Magic }

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHP = 100;
    [SerializeField] public float currentHP;
    [SerializeField] private float knockbackForce = 5f;

    // Публичное свойство для проверки смерти
    public bool IsDead { get; private set; }

    private Animator animator;

    private void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage, DamageType type)
    {
        if (IsDead) return;

        currentHP -= damage;
        animator.SetTrigger("Hurt");

        // Отбрасывание
        Vector3 knockbackDirection = -transform.forward * knockbackForce;
        transform.position += knockbackDirection;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true; // Устанавливаем флаг смерти
        animator.SetTrigger("Die");
        if (CompareTag("Player"))
        {
            UIManager.Instance.ShowDeathScreen();
        }
        else
        {
            Destroy(gameObject, 3f);
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}