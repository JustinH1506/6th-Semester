using UnityEngine;

public class PlayerAttackSecondState : PlayerBaseState
{
	public PlayerAttackSecondState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState()
	{
		ctx.Anim.Play(PlayerAnimationFactory.AttackAnim02);
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
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			return;
		}
		
		ctx.IsAttacking = false;

		if (ctx.AttackAmount >= 1)
		{
			SwitchStates(factory.AttackThird());
		}
		else if (ctx.IsMoving && ctx.IsSprinting)
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
