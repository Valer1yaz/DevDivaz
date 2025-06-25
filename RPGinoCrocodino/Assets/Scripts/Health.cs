using UnityEngine;

public enum DamageType { Physical, Magic }

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHP;
    [SerializeField] public float currentHP;
    [SerializeField] private float knockbackForce = 2f;

    // Публичное свойство для проверки смерти
    public bool IsDead { get; private set; }

    private Animator animator;

    private void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public float CurrentHealth
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
            if (CompareTag("Player") && UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI(currentHP, maxHP);
            }
            if (currentHP <= 0 && !IsDead)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage, DamageType type)
    {
        if (IsDead) return;

        currentHP -= damage;

        // Если это игрок - обновляем UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealthUI(currentHP, maxHP);
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }

        // Отбрасывание (для игрока и мобов)
        Vector3 knockbackDirection = -transform.forward * knockbackForce;
        transform.position += knockbackDirection;

        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        IsDead = true; // Устанавливаем флаг смерти
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        if (CompareTag("Player"))
        {
            if (UIManager.Instance != null)
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