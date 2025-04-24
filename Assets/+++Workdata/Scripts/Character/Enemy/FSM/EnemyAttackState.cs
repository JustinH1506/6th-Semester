using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
	public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	private float waitCounter;
	private float maxWaitCounter = 2f;
	public override void EnterState()
	{
		FaceTarget();
		waitCounter = maxWaitCounter;
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
		ctx.Anim.SetFloat("Distance", Vector3.Distance(ctx.transform.position, ctx.PlayerTransform.position));
	}

	public override void ExitState()
	{
		waitCounter = maxWaitCounter;
		ctx.NavMeshAgent.isStopped = false;
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
		{
			return;
		}
		
		if (ctx.DistanceBetweenPlayer() < ctx.FollowDistance)
		{
			// while (waitCounter > 0.05f)
			// {
			// 	waitCounter -= Time.deltaTime;
			// 	return;
			// }
			
			SwitchStates(factory.Follow());
		}
		else if(ctx.DistanceBetweenPlayer() > ctx.PatrolDistance)
		{
			SwitchStates(factory.Patrol());
		}
	}
	
	void FaceTarget()
	{
		var turnTowardNavSteeringTarget = ctx.NavMeshAgent.steeringTarget;
      
		Vector3 direction = (turnTowardNavSteeringTarget - ctx.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		
		ctx.transform.rotation = Quaternion.Slerp(ctx.transform.rotation, lookRotation, Time.deltaTime * 500);
	}
}
