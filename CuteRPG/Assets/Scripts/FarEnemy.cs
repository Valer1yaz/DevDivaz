using UnityEngine;

public class FarEnemy : MonoBehaviour
{
    public int damage = 15;
    public float attackRange = 10f;
    public float attackCooldown = 3f;
    public Transform player;
    public Animator animator;
    public GameObject magicProjectile; // ������ ����������� �������

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
        animator.SetTrigger("Attack"); // �������� �����
        GameObject projectile = Instantiate(magicProjectile, transform.position, Quaternion.identity);
        projectile.GetComponent<MagicProjectile>().SetTarget(player);
    }
}