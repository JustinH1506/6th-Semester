using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
	public EnemyStunState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}
    
	public override void EnterState()
	{
		ctx.Anim.Play(EnemyAnimationFactory.Hit);
	}

	public override void UpdateState()
	{
		while(ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
		{
			return;
		}
		
		CheckSwitchStates();
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	public override void CheckSwitchStates()
	{
		if (Vector3.Distance(ctx.transform.position, ctx.PlayerTransform.position) < ctx.FollowDistance)
		{
			SwitchStates(factory.Follow());
		}
		else if(ctx.NavMeshAgent.remainingDistance > 5)
		{
			SwitchStates(factory.Patrol());
		}
	}
}
