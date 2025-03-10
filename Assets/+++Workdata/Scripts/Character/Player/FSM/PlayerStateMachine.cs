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
    [SerializeField] private float rotationSpeed = 0f;
    private float moveSpeed = 0;
    private bool isSprinting = false;


    #region Getters and Setters

    public float MaxMoveSpeed {get { return maxMoveSpeed; } set { maxMoveSpeed = value; } }
    public float MaxSprintMoveSpeed {get { return maxSprintMoveSpeed; } set { maxSprintMoveSpeed = value; } }
    public float MoveSpeed {get { return moveSpeed; } set { moveSpeed = value; } }
    public bool IsSprinting {get { return isSprinting; } }
    public bool IsMoving {get { return isMoving; } }

    #endregion
    [Space]
    
    #endregion
	
    [Header("Animations")] 
    private Animator anim;
    [Space]
	
    private Rigidbody rb;
    private float inputX;
    private float inputZ;
    private bool isMoving;

    private void Awake()
    {
	    states = new PlayerStateFactory(this);
	    currentState = states.Idle();
	    currentState.EnterState();
	    rb = GetComponent<Rigidbody>();
	    anim  = GetComponent<Animator>();
    }

    private void Start()
    {
	    moveSpeed = maxMoveSpeed;
    }

    private void FixedUpdate()
    {
	    currentState.UpdateState();
    }

    public void Move(InputAction.CallbackContext context)
    {
	    inputX = context.ReadValue<Vector3>().x;
	    inputZ = context.ReadValue<Vector3>().z;
	    if (inputX != 0 || inputZ != 0)
	    {
		    isMoving = true;
	    }
	    else
	    {
		    isMoving = false;
	    }
    }

    public void HandleMovement()
    {
	    Vector3 cameraForward = Camera.main.transform.forward;
	    Vector3 cameraRight = Camera.main.transform.right;

	    cameraForward.y = 0;
	    cameraRight.y = 0;
	    cameraForward = cameraForward.normalized;
	    cameraRight = cameraRight.normalized;
		
	    Vector3 forwardRelativeMovementVector = inputZ * cameraForward;
	    Vector3 rightRelativeMovementVector = inputX * cameraRight;
		
	    Vector3 cameraRelativeMovement = forwardRelativeMovementVector + rightRelativeMovementVector;
	    cameraRelativeMovement.Normalize();
		
	    transform.forward = Vector3.Slerp(transform.forward, cameraRelativeMovement.normalized, Time.deltaTime * rotationSpeed);
		
	    rb.linearVelocity = new Vector3(cameraRelativeMovement.x * moveSpeed, rb.linearVelocity.y, cameraRelativeMovement.z * moveSpeed);
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
