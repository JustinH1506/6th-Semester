using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState()
	{
		ctx.Anim.CrossFade(PlayerAnimationFactory.AttackAnim01, 0.01f);
	}

	public override void UpdateState()
	{
		ctx.HandleRotation(ctx.HandleCameraRelative(), 500f);
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ctx.CanTurn = true;
		ctx.AttackAmount = 0;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return;
		}
		
		ctx.IsAttacking = false;
		
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
	
	public override void ChangeAttackAnimation()
	{
		if (ctx.AttackAmount > 1)
		{
			SwitchStates(factory.AttackSecond());
		}
	}
	
	public override void InitializeSubStates(){}
}
