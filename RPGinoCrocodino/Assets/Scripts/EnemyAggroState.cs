using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;

    public EnemyAggroState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        enemyAI.animator.SetBool("IsMoving", true);
        enemyAI.animator.ResetTrigger("Attack");
    }

    public void Execute()
    {
        if (enemyAI.IsPeaceful)
        {
            stateMachine.ChangeState(new EnemyIdleState(enemyAI, stateMachine));
            return;
        }

        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);

        // Проверяем тип врага
        if (enemyAI.enemyType == EnemyType.Melee || enemyAI.isBoss)
        {

            if (distance > enemyAI.attackRange)
            {
                enemyAI.ChasePlayer();
            }
            else if (distance <= enemyAI.attackRange)
            {
                stateMachine.ChangeState(new EnemyAttackState(enemyAI, stateMachine));
            }
            if (distance > enemyAI.attackRange + 4f)
            {
                stateMachine.ChangeState(new EnemyIdleState(enemyAI, stateMachine));
            }
        }
        else if (enemyAI.enemyType == EnemyType.Ranged)
        {

            if (distance >= 5f && distance <= 20f)
            {
                enemyAI.animator.SetBool("IsMoving", false);
                enemyAI.Attack();
            }
            else if (distance < 5f)
            {
                enemyAI.Retreat();
            }
            else
            {
                enemyAI.animator.SetBool("IsMoving", false);
            }
        }
    }

    public void Exit()
    {
        enemyAI.animator.SetBool("IsMoving", false);
    }
}