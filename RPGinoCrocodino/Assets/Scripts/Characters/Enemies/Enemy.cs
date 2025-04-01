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

        // Instantiate health bar
        GameObject healthBarObj = Instantiate(healthBarPrefab, healthBarPosition.position, Quaternion.identity, transform);
        healthBar = healthBarObj.GetComponent<EnemyHealthBar>();
        healthBar.Initialize(maxHealth);

        //проверки
        if (healthBarPrefab == null || healthBarPosition == null)
        {
            Debug.LogError("Отсутствует ссылка на HealthBar!", this);
            return;
        }

        GameObject healthBarObj = Instantiate(healthBarPrefab,
            healthBarPosition.position,
            Quaternion.identity,
            transform);

        if (!healthBarObj.TryGetComponent(out healthBar))
        {
            Destroy(healthBarObj);
            Debug.LogError("У префаба HealthBar отсутствует компонент EnemyHealthBar!", this);
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

    protected abstract void MoveTowardsPlayer();
    protected abstract void Attack();

    public virtual void TakeDamage(int amount, DamageType damageType)
    {
        if (isDead) return;

        currentHealth -= amount;
        healthBar.UpdateHealth(currentHealth);

        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
        healthBar.gameObject.SetActive(false);
    }
}