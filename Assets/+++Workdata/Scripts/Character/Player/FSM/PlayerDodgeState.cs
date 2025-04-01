using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}

	
	public override void EnterState()
	{
		ctx.Anim.SetBool("IsDodging", true);
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void ExitState()
	{
		ctx.IsDodging = false;
	}

	public override void CheckSwitchStates()
	{
		ctx.Anim.SetBool("IsDodging", true);
		
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			return;
		}

		ctx.IsDodging = false;
		
		if (ctx.IsMoving && ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
		else if (ctx.IsMoving && !ctx.IsSprinting)
		{
			SwitchStates(factory.Walk());
		}
		else
		{
			SwitchStates(factory.Idle());
		}
	}
	public override void InitializeSubStates(){}
}
