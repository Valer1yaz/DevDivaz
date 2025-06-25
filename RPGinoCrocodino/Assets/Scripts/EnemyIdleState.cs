using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;
    private Health health;

    public EnemyIdleState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        enemyAI.animator.SetBool("IsMoving", false);
        enemyAI.animator.ResetTrigger("Attack");
        health = enemyAI.gameObject.GetComponent<Health>();

    }

    public void Execute()
    {
        float hpPercent = health.currentHP / health.maxHP;
        if (enemyAI.IsPeaceful())
        {
            if (hpPercent > 0.3f)
                return;
            if (hpPercent <= 0.3f && !enemyAI.isBoss)
            {
                stateMachine.ChangeState(new EnemyFleeState(enemyAI, stateMachine));
            }
            if (health.currentHP < health.maxHP && enemyAI.isBoss)
            {
                enemyAI.OnAttacked();
            }

        }
            
        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        if (distance < enemyAI.AggroRange && !enemyAI.IsPeaceful())
        {
            stateMachine.ChangeState(new EnemyAggroState(enemyAI, stateMachine));
        }
    }

    public void Exit() { }
}