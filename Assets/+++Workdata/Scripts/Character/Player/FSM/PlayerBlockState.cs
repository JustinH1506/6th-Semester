using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
	public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}

	
	public override void EnterState()
	{
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		if (ctx.IsMoving && ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
		else if (ctx.IsMoving && !ctx.IsSprinting)
		{
			SwitchStates(factory.Walk());
		}
		else if(!ctx.IsAttacking)
		{
			SwitchStates(factory.Idle());
		}
	}
	public override void InitializeSubStates(){}
}
