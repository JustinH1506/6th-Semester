using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
	public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :base(currentContext, playerStateFactory){}
	
	public override void EnterState( )
	{
		
	}

	public override void UpdateState()
	{
		HandleMovement();
		CheckSwitchStates();
	}

	public override void ExitState()
	{
	}

	public override void CheckSwitchStates()
	{
		if (ctx.IsAttacking)
		{
			SwitchStates(factory.Attack());
			ctx.Rb.linearVelocity = Vector3.zero;
		}
		else if (!ctx.IsMoving && !ctx.IsSprinting)
		{
			SwitchStates(factory.Idle());
		}
		else if (ctx.IsSprinting)
		{
			SwitchStates(factory.Run());
		}
	}
	public override void InitializeSubStates(){}
	
	public void HandleMovement()
	{
		Vector3 cameraForward = Camera.main.transform.forward;
		Vector3 cameraRight = Camera.main.transform.right;

		cameraForward.y = 0;
		cameraRight.y = 0;
		cameraForward = cameraForward.normalized;
		cameraRight = cameraRight.normalized;
		
		Vector3 forwardRelativeMovementVector = ctx.InputZ * cameraForward;
		Vector3 rightRelativeMovementVector = ctx.InputX * cameraRight;
		
		Vector3 cameraRelativeMovement = forwardRelativeMovementVector + rightRelativeMovementVector;
		cameraRelativeMovement.Normalize();
		
		ctx.transform.forward = Vector3.Slerp(ctx.transform.forward, cameraRelativeMovement.normalized, Time.deltaTime * ctx.RotationSpeed);
		
		ctx.Rb.linearVelocity = new Vector3(cameraRelativeMovement.x * ctx.MoveSpeed, ctx.Rb.linearVelocity.y, cameraRelativeMovement.z * ctx.MoveSpeed);
	}
}
