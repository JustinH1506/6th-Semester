using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
	public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	private float waitCounter;
	private float maxWaitCounter = 2f;
	public override void EnterState()
	{
		ctx.Anim.Play(EnemyAnimationFactory.Attack);
		FaceTarget();
		waitCounter = maxWaitCounter;
		ctx.AttackCooldown = ctx.MaxAttackCooldown;
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		waitCounter = maxWaitCounter;
		ctx.NavMeshAgent.isStopped = false;

		ctx.StartCoroutine(ctx.HandleAttackCooldown());
	}

	public override void CheckSwitchStates()
	{
		if (ctx.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			return;
		}
		
		if (ctx.DistanceBetweenPlayer() < ctx.FollowDistance)
		{
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
