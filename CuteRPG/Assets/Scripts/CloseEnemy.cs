using UnityEngine;

public class CloseEnemy : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public Transform player;
    public Animator animator;

    private float lastAttackTime;

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack"); // Анимация атаки
        player.GetComponent<Health>().TakeDamage(damage);
    }
}