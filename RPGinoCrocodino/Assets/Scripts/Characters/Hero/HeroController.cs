using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class HeroController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float physicalAttackRange = 1.5f;
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Magic System")]
    [SerializeField] private MagicSystem magicSystem;

    private CharacterController characterController;
    private Animator animator;
    private Camera mainCamera;
    private Vector3 movement;
    private bool isRunning;
    private bool isAttacking;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
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
        if (IsInHitStun()) return;

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

    // Called from animation event
    private void PerformPhysicalAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, physicalAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(physicalDamage, DamageType.Physical);
            }
        }
    }

    // Called from animation event
    private void SpawnMagicProjectile()
    {
        Instantiate(magicProjectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    // Called from animation event
    private void EndAttack()
    {
        isAttacking = false;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", movement.magnitude * (isRunning ? 2 : 1));
        animator.SetBool("IsRunning", isRunning);
    }

    // Спавн через пул объектов
    GameObject projectile = ObjectPooler.Instance.SpawnFromPool("MagicProjectile",
        projectileSpawnPoint.position,
        projectileSpawnPoint.rotation);
        
    if (projectile.TryGetComponent(out MagicProjectile magicProj))
    {
        magicProj.Initialize(transform.forward, GetComponent<DamageDealer>());
    }
}
}