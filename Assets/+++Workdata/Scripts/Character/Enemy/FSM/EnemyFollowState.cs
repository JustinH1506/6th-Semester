using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	public override void EnterState(){}

	public override void UpdateState()
	{
		CheckSwitchStates();
		
		ctx.NavMeshAgent.SetDestination(ctx.PlayerTransform.position);

		ctx.Anim.SetFloat("Distance", ctx.NavMeshAgent.remainingDistance);
	}

	public override void ExitState()
	{
		
	}
	public override void CheckSwitchStates(){}

}
