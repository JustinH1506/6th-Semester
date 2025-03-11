using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void ExitState()
	{
		ctx.AttackAmount = 0;
		ctx.Anim.SetInteger("CurrentAttack", ctx.AttackAmount);
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack") ||  ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack02"))
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
	public override void InitializeSubStates(){}
}
