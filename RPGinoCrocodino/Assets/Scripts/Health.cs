using UnityEngine;

public enum DamageType { Physical, Magic }

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHP;
    [SerializeField] public float currentHP;
    [SerializeField] private float knockbackForce = 2f;

    // ��������� �������� ��� �������� ������
    public bool IsDead { get; private set; }

    private Animator animator;

    private EnemySpawnManager spawnManager;

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

        // ���� ��� ����� - ��������� UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealthUI(currentHP, maxHP);
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }

        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null && enemyAI.isBoss && enemyAI.IsPeaceful())
        {
            enemyAI.OnAttacked();
        }

        // ������������ (��� ������ � �����)
        Vector3 knockbackDirection = -transform.forward * knockbackForce;
        transform.position += knockbackDirection;

        if (currentHP <= 0) Die();
    }

    public void SetSpawnManager(EnemySpawnManager manager)
    {
        spawnManager = manager;
    }

    private void Die()
    {
        IsDead = true; // ������������� ���� ������
        
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
            if (spawnManager != null)
            {
                spawnManager.EnemyKilled(gameObject);
                Debug.Log("���� ���� � ��������� ��������");
            }
            else
            {
                Debug.LogWarning("spawnManager �� ��������");
            }
            Destroy(gameObject, 0.3f);
        }
    }

}