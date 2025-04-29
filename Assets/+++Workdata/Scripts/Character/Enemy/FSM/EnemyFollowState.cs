using UnityEngine;

public class EnemyFollowState: EnemyBaseState
{
	public EnemyFollowState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	public override void EnterState()
	{
		ctx.Anim.Play(EnemyAnimationFactory.Walk);
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}
	
	public override void FixedUpdateState()
	{
		ctx.NavMeshAgent.SetDestination(ctx.PlayerTransform.position);
	}

	public override void ExitState()
	{
		ctx.CanAttack = false;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.DistanceBetweenPlayer() < ctx.AttackDistance && ctx.CanAttack)
		{
			ctx.NavMeshAgent.isStopped = true;
			SwitchStates(factory.Attack());
		}
		else if(ctx.NavMeshAgent.remainingDistance > ctx.PatrolDistance)
		{
			SwitchStates(factory.Patrol());
		}
	}
}
