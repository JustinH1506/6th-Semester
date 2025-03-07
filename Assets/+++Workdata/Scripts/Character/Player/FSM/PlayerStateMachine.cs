using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    #region States
    
    private PlayerBaseState currentState;
    private PlayerStateFactory states;
    
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    
    #endregion
    
    #region Movement Variabels
    
    [Header("Movement Variables"), Tooltip("Max movement speed during walking.")] 
    [SerializeField] private float maxMoveSpeed = 0;
    [SerializeField] private float maxSprintMoveSpeed = 0;
    private float moveSpeed = 0;
    private bool isSprinting = false;
    [Space]
    
    #endregion
	
    [Header("Animations")] 
    private Animator anim;
    [Space]
	
    private Rigidbody rb;
    private float inputX;
    private float inputZ;

    private void Awake()
    {
	    states = new PlayerStateFactory(this);
	    currentState = states.Idle();
	    currentState.EnterState();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
	    inputX = context.ReadValue<Vector3>().x;
	    inputZ = context.ReadValue<Vector3>().z;
    }

    public void Attack(InputAction.CallbackContext context)
    {
	    anim.SetTrigger("Attack");
    }
    
    public void Sprint(InputAction.CallbackContext context)
    {
	    if (!isSprinting)
	    {
		    isSprinting = true;
		    moveSpeed = maxSprintMoveSpeed;
	    }
	    else
	    {
		    isSprinting = false;
		    moveSpeed = maxMoveSpeed;
	    }
    }
}
