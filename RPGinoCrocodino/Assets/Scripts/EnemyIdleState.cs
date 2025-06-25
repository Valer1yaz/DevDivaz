using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;

    public EnemyIdleState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        enemyAI.animator.SetBool("IsMoving", false);
        enemyAI.animator.ResetTrigger("Attack");

    }

    public void Execute()
    {
        //if (enemyAI.IsPeaceful || (enemyAI.isBoss && enemyAI.Boss != null && !enemyAI.Boss.IsAttacked))
            //return;

        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        if (distance < enemyAI.AggroRange && !enemyAI.IsPeaceful)
        {
            stateMachine.ChangeState(new EnemyAggroState(enemyAI, stateMachine));
        }
    }

    public void Exit() { }
}