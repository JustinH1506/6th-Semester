using UnityEngine;

public class PlayerStateFactory
{
    private PlayerStateMachine context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }
    
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }
    
    public PlayerBaseState Run()
    {
        return new PlayerRunState(context, this);
    }
    
    public PlayerBaseState Attack()
    {
        return new PlayerAttackState(context, this);
    }

    public PlayerBaseState Block()
    {
        return new PlayerBlockState(context, this);
    }
}
