using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState()
	{
		ctx.Anim.SetBool("IsAttacking", false);
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
		ctx.Anim.SetInteger("CurrentAttack", ctx.AttackAmount);
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") ||  ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
		{
			return;
		}
		
		ctx.IsAttacking = false;
		ctx.Anim.SetBool("IsAttacking", false);

		
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
