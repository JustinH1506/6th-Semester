using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState( )
	{
		ctx.Anim.Play(PlayerAnimationFactory.RunAnim);
	}

	public override void UpdateState()
	{
		
	}
	
	public override void FixedUpdateState()
	{
		ctx.HandleMovement();
		ctx.Stamina -= Time.deltaTime * ctx.RunCost;
		CheckSwitchStates();
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
		else if (!ctx.IsSprinting && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
			
		}
		else if (!ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
		else if (ctx.IsDodging)
		{
			SwitchStates(factory.Dodge());
		}
		else if (ctx.Stamina <= 0 && ctx.IsMoving)
		{
			SwitchStates(factory.Walk());
		}
		else if (ctx.Stamina <= 0 && !ctx.IsMoving)
		{
			SwitchStates(factory.Idle());
		}
	}
	public override void InitializeSubStates(){}
}
