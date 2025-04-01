using UnityEngine;

[System.Serializable]
public class HeroStats : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int physicalDamage = 10;
    [SerializeField] private int magicDamage = 15;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private float hitStunDuration = 0.3f;
    [SerializeField] private float knockbackForce = 5f;

    private int currentHealth;
    private float hitStunTimer;
    private bool isDead;
    private Animator animator;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (hitStunTimer > 0)
        {
            hitStunTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount, DamageType damageType, Transform damageSource = null)
    {
        if (isDead) return;

        currentHealth -= amount;
        hitStunTimer = hitStunDuration;

        // Расчет направления отбрасывания
        Vector3 knockbackDirection = damageSource != null
            ? (transform.position - damageSource.position).normalized
            : -transform.forward;

        knockbackDirection.y = 0;

        // Применение отбрасывания
        if (TryGetComponent<CharacterController>(out var controller))
        {
            controller.Move(knockbackDirection * knockbackForce * Time.deltaTime);
        }

        // Визуальные эффекты
        if (hitEffect != null)
        {
            hitEffect.Play();
        }

        // Обновление UI
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GameManager.Instance.PlayerDied();
    }

    public bool IsInHitStun()
    {
        return hitStunTimer > 0;
    }
}