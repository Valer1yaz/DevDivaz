using UnityEngine;
using System.Collections;

public enum EnemyType { Melee, Ranged }

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private ParticleSystem deathEffect;

    [Header("Ranged Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private ParticleSystem castEffect; // ������ ������

    [Header("Melee Attack")]
    [SerializeField] private float meleeDamageRadius = 1.5f; // ������ ������� �����
    [SerializeField] private LayerMask playerLayer; // ���� ������


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
        HandleRotation();
        HandleCombat(distance);
    }

    private void HandleRotation()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    private void HandleCombat(float distance)
    {
        if (enemyType == EnemyType.Melee)
        {
            if (distance <= attackRange) Attack();
            else ChasePlayer();
        }
        else if (enemyType == EnemyType.Ranged)
        {
            if (distance <= 20f && distance >= 5f) {
                animator.SetBool("IsMoving", false);
                Attack();
            }
            else if (distance < 5f) Retreat();
            else ChasePlayer();
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

            if (enemyType == EnemyType.Ranged)
            {
                StartCoroutine(RangedAttackSequence());
            }
            else if (enemyType == EnemyType.Melee)
            {
                // ����� ������ � ������� �����
                Collider[] hitPlayers = Physics.OverlapSphere(
                    transform.position,
                    meleeDamageRadius,
                    playerLayer
                );

                foreach (Collider player in hitPlayers)
                {
                    if (player.CompareTag("Player"))
                    {
                        player.GetComponent<Health>().TakeDamage(damage, DamageType.Physical);
                    }
                }
            }

            cooldownTimer = 0f;
        }
    }

    private IEnumerator RangedAttackSequence()
    {
        // ������ ������
        if (castEffect != null) castEffect.Play();

        // �������� ����� ��������� (������������� � ���������)
        yield return new WaitForSeconds(1f);

        // �������� �������
        GameObject projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.LookRotation(player.position - projectileSpawnPoint.position)
        );

        // ��������� �������
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(
            damage,
            DamageType.Magic,
            projectileSpeed,
            player.position + Vector3.up * 1f // ����: ����� ������
        );
        Destroy(projectile, 5f);
    }

    public void OnDeath()
    {
        isDead = true;
        // ������ ������
        if (deathEffect != null) deathEffect.Play();
        Destroy(GetComponent<EnemyAI>());
        Destroy(gameObject, 3f);
    }
}