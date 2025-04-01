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
            // Отойти от игрока если он слишком близко
            direction = (transform.position - player.position).normalized;
        }
        else
        {
            // Подойти к игроку сохраняя дистанцию
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

            // Снаряд появляется через событие анимации
        }
    }

    // Используем пул объектов
    private void ShootProjectile()
    {
        GameObject projectile = ObjectPooler.Instance.SpawnFromPool("EnemyProjectile",
            projectileSpawnPoint.position,
            Quaternion.LookRotation(player.position - projectileSpawnPoint.position));

        if (projectile.TryGetComponent(out EnemyProjectile proj))
        {
            proj.Initialize(GetComponent<DamageDealer>());
        }
    }
}