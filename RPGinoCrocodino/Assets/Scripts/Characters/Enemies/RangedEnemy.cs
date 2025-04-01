using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fleeDistance = 5f;

    protected override void MoveTowardsPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 direction;

        if (distanceToPlayer < fleeDistance)
        {
            // Flee from player if too close
            direction = (transform.position - player.position).normalized;
        }
        else
        {
            // Move towards player but keep distance
            direction = (player.position - transform.position).normalized;
        }

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

            // Projectile will be spawned via animation event
        }
    }

    // Called from animation event
    private void ShootProjectile()
    {
        if (player == null) return;

        Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(player.position - projectileSpawnPoint.position));
    }
}