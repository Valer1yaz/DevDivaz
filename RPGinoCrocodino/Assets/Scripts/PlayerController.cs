using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f; // ���������������� ����
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private bool isRunning;
    private float verticalVelocity;
    private float rotationX; // ���� �������� ������ �� ���������

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject magicAOEEffect; //��� ���������� �����������
    [SerializeField] public float magicAttackRadius = 5f; // ����� �������� �������
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

        // ������ � ������������� ������
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
        // ������� ��������� � ������ �� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ������� ��������� �� �����������
        transform.Rotate(0, mouseX, 0);

        // ������� ������ �� ��������� (�����������)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -30f, 30f); // ����������� ����
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
    private void HandleMovement()
    {
        // �������� ������/����� (W/S)
        float vertical = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // ����������� �������� ������ ������ ������������ ���������
        Vector3 move = transform.forward * vertical;

        // ���������� ��������
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        moveDirection = move * currentSpeed;

        // ����������
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // ��������
        controller.Move(
            (moveDirection + Vector3.up * verticalVelocity) * Time.deltaTime
        );

        // �������� ��������
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
            MagicalAOEAttack(); // �������� ������� �� AOE
        }
    }

    private void MagicalAOEAttack()
    {
        var effect = Instantiate(magicAOEEffect, transform.position, Quaternion.identity);
        Collider[] hitEnemies = Physics.OverlapSphere(
            transform.position, // ����� ����� - ��� �����
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

    // ������������ ������� � ���������
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