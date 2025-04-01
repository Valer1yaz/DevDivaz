using UnityEngine;

public class MeleeEnemy : Enemy
{
    protected override void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        animator.SetBool("IsMoving", true);
    }

    protected override void Attack()
    {
        animator.SetBool("IsMoving", false);

        if (attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;

            // Damage will be applied via animation event
        }
    }

    // Called from animation event
    private void DealDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (player.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage, DamageType.Physical);
            }
        }
    }
}