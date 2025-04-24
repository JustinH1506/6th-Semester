using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	#region Variables
	
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	#endregion
	
	#region My Methods
	
	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		ctx.HandleRotation(ctx.HandleCameraRelative(), ctx.RotationSpeed);
		ctx.GetCurrentStamina();
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}
	
	public override void ExitState(){}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
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
	
	#endregion
}
