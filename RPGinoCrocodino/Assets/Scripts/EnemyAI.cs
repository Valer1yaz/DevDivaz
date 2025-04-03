using UnityEngine;

public enum EnemyType { Melee, Ranged }

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 10;

    [Header("Ranged Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 10f;

    private Transform player;
    private Animator animator;
    private Health health;
    private float cooldownTimer;
    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (isDead || health.IsDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Поворот к игроку
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Логика атаки
        if (enemyType == EnemyType.Melee)
        {
            if (distance <= attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (enemyType == EnemyType.Ranged)
        {
            if (distance <= 10f && distance >= 5f)
            {
                Attack();
            }
            else if (distance < 5f)
            {
                Retreat();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        animator.SetBool("IsMoving", true);
    }

    private void Retreat()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        animator.SetBool("IsMoving", true);
    }

    private void Attack()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack");

            if (enemyType == EnemyType.Melee)
            {
                // Ближняя атака
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    player.GetComponent<Health>().TakeDamage(damage, DamageType.Physical);
                }
            }
            else if (enemyType == EnemyType.Ranged)
            {
                // Создание снаряда
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    projectileSpawnPoint.position,
                    Quaternion.identity
                );
                projectile.GetComponent<Rigidbody>().velocity =
                    (player.position - projectileSpawnPoint.position).normalized * projectileSpeed;
                projectile.GetComponent<Projectile>().SetDamage(damage);
            }

            cooldownTimer = 0f;
        }
    }

    public void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(GetComponent<EnemyAI>());
        Destroy(gameObject, 3f);
    }
}