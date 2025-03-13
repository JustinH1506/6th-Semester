using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
	public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	private float waitCounter;
	private float maxWaitCounter = 2f;
	public override void EnterState()
	{
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
	}

	public override void CheckSwitchStates()
	{
		while (waitCounter > 0.05f)
		{
			waitCounter -= Time.deltaTime;
			return;
		}
		
		SwitchStates(factory.Follow());
	}
}
