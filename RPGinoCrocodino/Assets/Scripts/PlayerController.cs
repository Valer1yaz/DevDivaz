using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private bool isRunning;

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform magicSpawnPoint;

    private Animator animator;
    private Health health;
    private MagicSystem magicSystem;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        magicSystem = GetComponent<MagicSystem>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    private void HandleMovement()
    {
        // Получение ввода
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector2(horizontal, vertical);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Поворот персонажа в сторону камеры (только по оси Y)
        Vector3 cameraFlatDirection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(cameraFlatDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Расчет движения относительно камеры
        Vector3 moveRelativeToCamera =
            cameraTransform.forward * inputDirection.y +
            cameraTransform.right * inputDirection.x;

        moveRelativeToCamera.y = 0; // Игнорируем вертикальную составляющую
        moveRelativeToCamera.Normalize();

        // Применение скорости
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        moveDirection = moveRelativeToCamera * currentSpeed;

        // Движение и гравитация
        controller.Move(moveDirection * Time.deltaTime +
                       Vector3.up * Physics.gravity.y * Time.deltaTime);

        // Обновление анимации
        animator.SetFloat("Speed", moveDirection.magnitude / runSpeed);
    }

    private void HandleCombat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            PhysicalAttack();
        }

        if (Input.GetMouseButtonDown(1) && magicSystem.CanUseMagic())
        {
            animator.SetTrigger("MagicAttack");
            magicSystem.UseMagic();
            Instantiate(magicProjectilePrefab, magicSpawnPoint.position, transform.rotation);
        }
    }

    private void PhysicalAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(10, DamageType.Physical);
        }
    }
}