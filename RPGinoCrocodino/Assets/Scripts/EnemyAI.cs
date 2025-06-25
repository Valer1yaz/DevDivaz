using UnityEngine;
using System.Collections;

public enum EnemyType { Melee, Ranged }

public class EnemyAI : MonoBehaviour
{
    public EnemyType enemyType;
    public bool isBoss = false; // Установите в инспекторе для босса
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int damage = 10;
    public ParticleSystem deathEffect;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public ParticleSystem castEffect;

    [Header("Melee Attack")]
    public float meleeDamageRadius = 1.5f;
    public LayerMask playerLayer;

    [Header("Behavior Settings")]
    public float AggroRange = 10f; // радиус агрессии
    public bool IsPeaceful = false; // режим мирности

    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public float CooldownTimer;

    // Для босса
    //public BossAI Boss { get; set; }
    public bool IsAttacked { get; private set; } = false;

    private Health health;
    public Animator animator { get; private set; }

    private EnemyStateMachine stateMachine;
    private bool isDead = false;

    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        stateMachine = GetComponent<EnemyStateMachine>();
        if (stateMachine == null)
            stateMachine = gameObject.AddComponent<EnemyStateMachine>();

        stateMachine.ChangeState(new EnemyIdleState(this, stateMachine));
    }

    private void Update()
    {
        if (isDead || health.IsDead) return;
        stateMachine.Update();
    }

    public void ChasePlayer()
    {
        Vector3 dir = (PlayerTransform.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void Retreat()
    {
        Vector3 dir = (transform.position - PlayerTransform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        CooldownTimer += Time.deltaTime;
        if (CooldownTimer >= attackCooldown)
        {
            if (enemyType == EnemyType.Ranged)
            {
                StartCoroutine(RangedAttackSequence());
            }
            else if (enemyType == EnemyType.Melee)
            {
                Collider[] hitPlayers = Physics.OverlapSphere(transform.position, meleeDamageRadius, playerLayer);
                foreach (var p in hitPlayers)
                {
                    if (p.CompareTag("Player"))
                        p.GetComponent<Health>().TakeDamage(damage, DamageType.Physical);
                }
            }
            CooldownTimer = 0f;
        }
    }

    private IEnumerator RangedAttackSequence()
    {
        if (castEffect != null) castEffect.Play();
        yield return new WaitForSeconds(1f);
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(PlayerTransform.position - projectileSpawnPoint.position));
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.Initialize(damage, DamageType.Magic, projectileSpeed, PlayerTransform.position + Vector3.up * 1f);
        Destroy(projectile, 5f);
    }

    public void OnDeath()
    {
        isDead = true;
        if (deathEffect != null) deathEffect.Play();
        Destroy(gameObject, 3f);
    }

    // Метод для реакции на удар
    public void OnAttacked()
    {

    }
}