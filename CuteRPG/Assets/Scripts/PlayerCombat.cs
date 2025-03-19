using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int physicalDamage = 10;
    public int magicDamage = 15;
    public float magicCooldown = 5f;
    public float attackRange = 2f; // Дистанция атаки
    public LayerMask enemyLayer; // Слой для поиска врагов
    public Animator animator;

    private float lastMagicAttackTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            PerformPhysicalAttack();
        }
        if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
        {
            if (Time.time - lastMagicAttackTime >= magicCooldown)
            {
                PerformMagicAttack();
                lastMagicAttackTime = Time.time;
            }
        }
    }

    private void PerformPhysicalAttack()
    {
        animator.SetTrigger("Attack"); // Анимация физической атаки
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(physicalDamage);
        }
    }

    private void PerformMagicAttack()
    {
        animator.SetTrigger("MagicAttack"); // Анимация магической атаки
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(magicDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация радиуса атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}