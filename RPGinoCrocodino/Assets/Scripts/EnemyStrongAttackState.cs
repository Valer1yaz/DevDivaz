using UnityEngine;
public class EnemyStrongAttackState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;

    public EnemyStrongAttackState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        enemyAI.animator.SetTrigger("StrongAttack");
        enemyAI.animator.ResetTrigger("Attack");
        enemyAI.CooldownTimer = 2f;
    }

    public void Execute()
    {
        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        enemyAI.PerformStrongAttackEffects();
        // ����� ����� ������, ��� ������ ������
        if (distance > enemyAI.attackRange)
        {
            stateMachine.ChangeState(new EnemyAggroState(enemyAI, stateMachine));
        }
        else
        {
            enemyAI.PerformNormalAttackEffects();
        }
    }

    public void Exit()
    {
        //enemyAI.animator.ResetTrigger("StrongAttack");
    }
}