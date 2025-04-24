using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
	#region Variables
	
	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	#endregion
	
	#region My Methods
	public override void EnterState( )
	{
		ctx.Anim.SetBool( "IsMoving", true );
	}

	public override void UpdateState()
	{
		ctx.GetCurrentStamina();
		ctx.HandleMovement();
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
			ctx.Rb.linearVelocity = Vector3.zero;
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
			ctx.Anim.SetBool( "IsMoving", false );
		}
		else if (ctx.IsMoving && ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
	}
	public override void InitializeSubStates(){}
	
	#endregion
}