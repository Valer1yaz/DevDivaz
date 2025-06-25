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
        enemyAI.CooldownTimer = 0f;
    }

    public void Execute()
    {
        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        enemyAI.Attack();
        // ����� ����� ������, ��� ������ ������
        if (distance > enemyAI.attackRange)
        {
            stateMachine.ChangeState(new EnemyAggroState(enemyAI, stateMachine));
        }
        else
        {
            enemyAI.Attack();
        }
    }

    public void Exit()
    {
        // ����� �������� �������, ���� �����
        // enemyAI.animator.ResetTrigger("StrongAttack");
    }
}