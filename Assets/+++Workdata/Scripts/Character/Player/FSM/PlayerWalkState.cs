using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
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
		if (!ctx.IsMoving && !ctx.IsSprinting)
		{
			SwitchStates(factory.Idle());
		}
		else if (ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
	}
	public override void InitializeSubStates(){}
}
