using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
		
		ctx.NavMeshAgent.SetDestination(ctx.PlayerTransform.position);
	}

	public override void ExitState()
	{
		ctx.NavMeshAgent.isStopped = true;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.DistanceBetweenPlayer() < 8)
		{
			SwitchStates(factory.Attack());
		}
		else if(ctx.NavMeshAgent.remainingDistance > 5)
		{
			SwitchStates(factory.Patrol());
		}
	}

}
