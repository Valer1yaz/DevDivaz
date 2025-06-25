using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;
    private Health health;

    public EnemyFleeState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        health = enemyAI.gameObject.GetComponent<Health>();
        enemyAI.animator.SetBool("IsMoving", true);
        enemyAI.animator.ResetTrigger("Attack");
    }

    public void Execute()
    {

        float hpPercent = health.currentHP / health.maxHP;
        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        
            if (hpPercent > 0.3f)
        {
            // Если здоровье выше 30%, возвращаемся в Idle или другое состояние
            stateMachine.ChangeState(new EnemyIdleState(enemyAI, stateMachine));
        }
        else
        {
            enemyAI.Retreat();
            if (distance >= 5f && distance <= 20f)
            {
                enemyAI.animator.SetBool("IsMoving", false);
            }
            else if (distance < 5f)
            {
                enemyAI.Retreat();
            }
            else
            {
                stateMachine.ChangeState(new EnemyIdleState(enemyAI, stateMachine));
            }
        }
    }   

    public void Exit() { }
}