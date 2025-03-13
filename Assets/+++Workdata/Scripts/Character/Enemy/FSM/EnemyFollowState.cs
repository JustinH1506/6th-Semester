using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	public override void EnterState(){}

	public override void UpdateState()
	{
		CheckSwitchStates();
		
		ctx.NavMeshAgent.SetDestination(ctx.PlayerTransform.position);
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		if (Vector3.Distance(ctx.transform.position, ctx.PlayerTransform.position) < 3)
		{
			SwitchStates(factory.Attack());
		}
		else if(ctx.NavMeshAgent.remainingDistance > 5)
		{
			SwitchStates(factory.Patrol());
		}
	}

}
