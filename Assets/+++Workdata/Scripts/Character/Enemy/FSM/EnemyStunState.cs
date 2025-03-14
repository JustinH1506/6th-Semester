using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
	private float stunDuration;
	private float maxStunDuration;
	
	public EnemyStunState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
    
	public override void EnterState()
	{
		stunDuration = maxStunDuration;
	}

	public override void UpdateState()
	{
		while (stunDuration > 0.05f)
		{
			stunDuration -= Time.deltaTime;
			return;
		}
		
		CheckSwitchStates();
	}

	public override void ExitState()
	{
       stunDuration = maxStunDuration;
	}

	public override void CheckSwitchStates()
	{
		
	}
}
