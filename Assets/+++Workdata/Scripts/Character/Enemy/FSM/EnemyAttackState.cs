using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
	public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

	public override void EnterState(){}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}
	public override void ExitState(){}
	public override void CheckSwitchStates(){}

}
