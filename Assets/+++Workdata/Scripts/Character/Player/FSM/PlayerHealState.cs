using UnityEngine;

public class PlayerHealState : PlayerBaseState
{
	public PlayerHealState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}

	
	public override void EnterState()
	{
		ctx.Anim.SetBool("IsHealing", true);
	}

	public override void UpdateState()
	{
		
	}
	
	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		
	}
	public override void InitializeSubStates(){}
}
