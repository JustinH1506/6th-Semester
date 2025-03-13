using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState( )
	{
		ctx.Anim.SetBool( "IsMoving", true );
		ctx.Anim.SetBool( "IsSprinting", true );
	}

	public override void UpdateState()
	{
		ctx.HandleMovement();
		CheckSwitchStates();
	}
	public override void ExitState(){}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
			ctx.Rb.linearVelocity = Vector3.zero;
			ctx.Anim.SetBool( "IsSprinting", false );
		}
		else if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
			ctx.Anim.SetBool( "IsSprinting", false );
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
			ctx.Anim.SetBool( "IsSprinting", false );
			ctx.Anim.SetBool( "IsMoving", false );
		}
	}
	public override void InitializeSubStates(){}
}
