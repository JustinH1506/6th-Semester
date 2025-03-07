using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	
	public override void EnterState()
	{
		Debug.Log("Entering Idle State");
	}

	public override void UpdateState(){}
	public override void ExitState(){}
	public override void CheckSwitchStates(){}
	public override void InitializeSubStates(){}
}
