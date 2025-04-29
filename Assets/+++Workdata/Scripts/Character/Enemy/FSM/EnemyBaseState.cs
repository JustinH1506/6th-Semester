using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateMachine ctx;
    protected EnemyStateFactory factory;

    protected EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    {
        ctx = currentContext;
        factory = enemyStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    protected void SwitchStates(EnemyBaseState newState)
    {
        ExitState();
        
        newState.EnterState();
        
        ctx.CurrentState = newState;
    }
}
