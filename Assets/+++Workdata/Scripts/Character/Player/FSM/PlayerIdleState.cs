using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	
	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}
	public override void ExitState(){}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
		}
		else if (ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Run());
		}
		else if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
		}
	}
	public override void InitializeSubStates(){}
}
