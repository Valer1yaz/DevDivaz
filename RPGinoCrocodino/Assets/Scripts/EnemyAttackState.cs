using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyAI enemyAI;
    private EnemyStateMachine stateMachine;
    private Health health;

    public EnemyAttackState(EnemyAI enemy, EnemyStateMachine machine)
    {
        enemyAI = enemy;
        stateMachine = machine;
    }

    public void Enter()
    {
        health = enemyAI.GetComponent<Health>();
        enemyAI.animator.SetTrigger("Attack");
        enemyAI.CooldownTimer = 0f;

        // Проигрываем эффекты один раз при начале атаки
        enemyAI.ResetEffectsFlag();
        enemyAI.TriggerNormalAttackEffects();
    }

    public void Execute()
    {
        if (enemyAI.IsPeaceful())
        {
            stateMachine.ChangeState(new EnemyIdleState(enemyAI, stateMachine));
            return;
        }

        float distance = Vector3.Distance(enemyAI.transform.position, enemyAI.PlayerTransform.position);
        if (distance > enemyAI.attackRange)
        {
            stateMachine.ChangeState(new EnemyAggroState(enemyAI, stateMachine));
        }
        else
        {
            float hpPercent = health.currentHP / health.maxHP;

            if (hpPercent < 0.3f)
            {
                stateMachine.ChangeState(new EnemyFleeState(enemyAI, stateMachine));
            }
            else
            {
                enemyAI.Attack();
            }
        }
    }

    public void Exit()
    {
        
    }
}