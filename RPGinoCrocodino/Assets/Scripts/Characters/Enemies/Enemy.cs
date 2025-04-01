using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 50;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackCooldown = 2f;

    [Header("Components")]
    [SerializeField] protected Transform healthBarPosition;
    [SerializeField] protected GameObject healthBarPrefab;

    protected Animator animator;
    protected Transform player;
    protected EnemyHealthBar healthBar;
    protected int currentHealth;
    protected float attackTimer;
    protected bool isDead;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        // Исправленная инициализация healthBar
        if (healthBarPrefab != null && healthBarPosition != null)
        {
            GameObject healthBarObj = Instantiate(healthBarPrefab,
                healthBarPosition.position,
                Quaternion.identity,
                transform);

            healthBar = healthBarObj.GetComponent<EnemyHealthBar>();
            if (healthBar == null)
            {
                Debug.LogError("EnemyHealthBar component missing on health bar prefab", this);
                Destroy(healthBarObj);
            }
        }
        else
        {
            Debug.LogError("Health bar references not set in Enemy", this);
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime;

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void LateUpdate()
    {
        if (healthBar != null && Camera.main != null)
        {
            healthBar.transform.position = healthBarPosition.position;
            healthBar.transform.rotation = Camera.main.transform.rotation;
        }
    }

    protected abstract void MoveTowardsPlayer();
    protected abstract void Attack();

    public void TakeDamage(int amount, DamageType damageType, Transform damageSource = null)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        // Анимация получения урона
        animator.SetTrigger("Hit");

        // Отбрасывание от источника повреждений
        if (damageSource != null)
        {
            Vector3 knockbackDirection = (transform.position - damageSource.position).normalized;
            knockbackDirection.y = 0;

            if (TryGetComponent<CharacterController>(out var controller))
            {
                controller.Move(knockbackDirection * 0.5f * Time.deltaTime);
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        Destroy(gameObject, 3f);
    }
}