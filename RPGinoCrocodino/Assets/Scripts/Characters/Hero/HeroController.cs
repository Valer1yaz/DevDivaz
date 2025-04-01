using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class HeroController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Combat")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float physicalAttackRange = 1.5f;
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private int physicalDamage = 20; // Добавляем это поле
    [SerializeField] private int magicDamage = 30;

    [Header("Magic System")]
    [SerializeField] private MagicSystem magicSystem;

    private CharacterController characterController;
    private Animator animator;
    private Camera mainCamera;
    private Vector3 movement;
    private bool isRunning;
    private bool isAttacking;
    private HeroStats heroStats;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        heroStats = GetComponent<HeroStats>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCombat();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        if (heroStats != null && heroStats.IsInHitStun()) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        movement = (cameraForward * vertical + cameraRight * horizontal).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (movement.magnitude > 0)
        {
            characterController.Move(movement * currentSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (movement.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleCombat()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            PhysicalAttack();
        }

        if (Input.GetMouseButtonDown(1) && !isAttacking)
        {
            MagicAttack();
        }
    }

    private void PhysicalAttack()
    {
        isAttacking = true;
        animator.SetTrigger("PhysicalAttack");

        // Attack logic will be handled in animation event
        // а че у тебя комменты на басятском а не на русском
    }

    private void MagicAttack()
    {
        if (!magicSystem.HasEnoughMagic()) return;

        isAttacking = true;
        animator.SetTrigger("MagicAttack");
        magicSystem.ConsumeMagic(1);
    }

    // Вызывается анимацией
    private void PerformPhysicalAttack()
    {
        if (attackPoint == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            physicalAttackRange,
            enemyLayer
        );

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(
                    physicalDamage,
                    DamageType.Physical,
                    transform
                );
            }
        }
    }

    private void SpawnMagicProjectile()
    {
        if (magicSystem == null || !magicSystem.HasEnoughMagic()) return;

        GameObject projectile = Instantiate(
            magicProjectilePrefab,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation
        );

        if (projectile.TryGetComponent(out MagicProjectile proj))
        {
            proj.Initialize(
                transform.forward,
                GetComponent<DamageDealer>()
            );
            magicSystem.ConsumeMagic(1);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", movement.magnitude * (isRunning ? 2 : 1));
        animator.SetBool("IsRunning", isRunning);
    }

}
