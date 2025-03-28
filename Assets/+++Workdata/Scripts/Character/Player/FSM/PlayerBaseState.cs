using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    
    protected PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubStates();

    protected void SwitchStates(PlayerBaseState newState)
    {
        ExitState();
        
        newState.EnterState();
        
        ctx.CurrentState = newState;
    }
    
    protected void SetSuperState(){}
    protected void SetSubState(){}
}
