using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState( )
	{
		
	}

	public override void UpdateState()
	{
		ctx.HandleMovement();
		CheckSwitchStates();
	}
	public override void ExitState(){}

	public override void CheckSwitchStates()
	{
		if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
	}
	public override void InitializeSubStates(){}
}
