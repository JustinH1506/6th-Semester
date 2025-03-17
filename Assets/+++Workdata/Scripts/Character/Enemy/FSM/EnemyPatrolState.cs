using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) :base  (currentContext, enemyStateFactory){}

    public override void EnterState()
    {
        ctx.NavMeshAgent.autoBraking = false;
        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        if (ctx.NavMeshAgent.remainingDistance <= 0.1f)
        {
            NextPoint();
        }
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (Vector3.Distance(ctx.transform.position, ctx.PlayerTransform.position) < ctx.DistanceThreshold)
        {
            SwitchStates(factory.Follow());
        }
    }

    private void NextPoint()
    {
        if (ctx.CheckPoints.Length == 0)
            return;
        
        ctx.NavMeshAgent.destination = ctx.CheckPoints[ctx.CurrentPoint].position;
        
        ctx.CurrentPoint = (ctx.CurrentPoint + 1) % ctx.CheckPoints.Length;
    }
}

