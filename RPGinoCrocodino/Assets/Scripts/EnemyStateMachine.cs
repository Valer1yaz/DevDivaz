using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }

    public void ChangeState(IEnemyState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.Execute();
    }
}