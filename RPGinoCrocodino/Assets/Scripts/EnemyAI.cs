using UnityEngine;
using System.Collections;

public enum EnemyType { Melee, Ranged }

public class EnemyAI : MonoBehaviour
{
    public EnemyType enemyType;
    public bool isBoss = false; // ���������� � ���������� ��� �����
    private bool isPeaceful = false; // ���������� ���������� ��� ������
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
    public float AggroRange = 10f;

    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public float CooldownTimer;

    private Health health;
    public Animator animator { get; private set; }
    private EnemyStateMachine stateMachine;
    private bool isDead = false;

    public GameObject[] strongAttackEffects; // ������� ��� ������� ����� (2 �������)
    public GameObject[] normalAttackEffects; // ������� ��� ������� ����� (2 �������)

    public AudioSource strongAttackSound; // ���� ��� ������� �����

    private bool effectsPlayed = false; // ���� ��� �������� ������� �������� �� ���� �����

    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        if (GetComponent<EnemyStateMachine>() == null)
            gameObject.AddComponent<EnemyStateMachine>();
        stateMachine = GetComponent<EnemyStateMachine>();

        stateMachine.ChangeState(new EnemyIdleState(this, stateMachine));
    }

    private void Update()
    {
        if (isDead || health.IsDead) return;
        stateMachine.Update();
    }

    // ����� ��� ��������� ������
    public void SetPeacefulMode(bool isPeaceful)
    {
        this.isPeaceful = isPeaceful;
    }

    // ���������� ����� ��� �������� ������
    public bool IsPeaceful()
    {
        return isPeaceful;
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
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position,
            Quaternion.LookRotation(PlayerTransform.position - projectileSpawnPoint.position));
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.Initialize(damage, DamageType.Magic, projectileSpeed, PlayerTransform.position + Vector3.up * 1f);
        Destroy(projectile, 5f);
    }

    // ����� �������� ��� ������� ����� (������������� ���� ���)
    public void TriggerNormalAttackEffects()
    {
        if (isBoss)
        {
            if (normalAttackEffects != null && normalAttackEffects.Length > 0)
            {
                int index = Random.Range(0, normalAttackEffects.Length);
                Instantiate(normalAttackEffects[index], transform.position + Vector3.up, Quaternion.identity);
            }
        }
        effectsPlayed = true;
    }

    // ����� �������� ��� ������� ����� (������������� ���� ���)
    public void TriggerStrongAttackEffects()
    {
        if (isBoss)
        {
            if (strongAttackEffects != null && strongAttackEffects.Length > 0)
            {
                int index = Random.Range(0, strongAttackEffects.Length);
                Instantiate(strongAttackEffects[index], transform.position + Vector3.up, Quaternion.identity);
            }
            if (strongAttackSound != null)
            {
                strongAttackSound.Play();
            }
        }
        effectsPlayed = true;
    }

    // �������� ����, ����� ������� ����� ���� ��������� ����� ��� ��������� �����
    public void ResetEffectsFlag()
    {
        effectsPlayed = false;
    }

    public void OnDeath()
    {
        isDead = true;
        if (deathEffect != null) deathEffect.Play();
        Destroy(gameObject, 3f);
    }

    // ������� �� ����
    public void OnAttacked()
    {
        if (isBoss)
        {
            Debug.Log("���� ������� ����, �������� ��������");
            SetPeacefulMode(false);
        }
        if (stateMachine != null)
            stateMachine.ChangeState(new EnemyAggroState(this, stateMachine));
    }
}