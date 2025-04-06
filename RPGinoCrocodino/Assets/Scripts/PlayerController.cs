using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f; // Чувствительность мыши
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private bool isRunning;
    private float verticalVelocity;
    private float rotationX; // Угол поворота камеры по вертикали

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject magicAOEEffect; //для добавления спецэффекта
    [SerializeField] public float magicAttackRadius = 5f; // Новый параметр радиуса
    [SerializeField] public float magicDamage = 15f;

    private Animator animator;
    private Health health;
    private MagicSystem magicSystem;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        magicSystem = GetComponent<MagicSystem>();

        // Скрыть и зафиксировать курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleCombat();
    }

    private void HandleRotation()
    {
        // Поворот персонажа и камеры от мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Поворот персонажа по горизонтали
        transform.Rotate(0, mouseX, 0);

        // Поворот камеры по вертикали (опционально)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -30f, 30f); // Ограничение угла
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
    private void HandleMovement()
    {
        // Движение вперед/назад (W/S)
        float vertical = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Направление движения всегда вперед относительно персонажа
        Vector3 move = transform.forward * vertical;

        // Применение скорости
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        moveDirection = move * currentSpeed;

        // Гравитация
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // Движение
        controller.Move(
            (moveDirection + Vector3.up * verticalVelocity) * Time.deltaTime
        );

        // Анимация движения
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
            MagicalAOEAttack(); // Заменяем выстрел на AOE
        }
    }

    private void MagicalAOEAttack()
    {
        var effect = Instantiate(magicAOEEffect, transform.position, Quaternion.identity);
        Collider[] hitEnemies = Physics.OverlapSphere(
            transform.position, // Центр атаки - сам игрок
            magicAttackRadius,
            enemyLayers
        );

        foreach (Collider enemy in hitEnemies)
        {
            StartCoroutine(ApplyDamageOverTime(enemy, 3, 1f));
        }

        Destroy(effect, 2f);
    }
    private IEnumerator ApplyDamageOverTime(Collider enemy, int ticks, float interval)
    {
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth == null) yield break;

        for (int i = 0; i < ticks; i++)
        {
            enemyHealth.TakeDamage(magicDamage / ticks, DamageType.Magic);
            yield return new WaitForSeconds(interval);
        }
    }

    // Визуализация радиуса в редакторе
    private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, magicAttackRadius);
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