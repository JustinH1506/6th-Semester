using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : CharacterBase
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
    
    [Space]
    #endregion

    #region Attack Variables

    private bool isAttacking = false;
    private int attackAmount = 0;

    #endregion
	
    #region Animations
    [Header("Animations")]
    private Animator anim;
    
    #endregion
    
    #region Getters and Setters

    #region Movement
    
    public float MaxMoveSpeed {get { return maxMoveSpeed; } set { maxMoveSpeed = value; } }
    public float MaxSprintMoveSpeed {get { return maxSprintMoveSpeed; } set { maxSprintMoveSpeed = value; } }
    public float MoveSpeed {get { return moveSpeed; } }
    public float RotationSpeed {get { return rotationSpeed; } }
    public bool IsSprinting {get { return isSprinting; } }
    public bool IsMoving {get { return isMoving; } }
    
    #endregion

    #region Attack

    public int AttackAmount {get { return attackAmount; } set { attackAmount = value; } }
    public bool IsAttacking {get { return isAttacking; }  set { isAttacking = value; } }
    
    #endregion
    
    #region Stuff
    
    public Rigidbody Rb { get { return rb; } }
    
    #endregion
    
    public Animator Anim { get { return anim; } }
    
    public float InputZ { get { return inputZ; } }
    public float InputX { get { return inputX; } }
    
    [Space]
    
    #endregion
    
    #region Stuff
    
    private Rigidbody rb;
    private float inputX;
    private float inputZ;
    private bool isMoving;
    
	#endregion
	
	#region Methods
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
    
    private void OnEnable()
    {
	    OnRegisterCurrentHealth(HealthChanged, true);
    }
    private void OnDisable()
    {
	    OnHealthChanged -= HealthChanged;
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
		    Anim.SetBool("IsMoving", true);
	    }
	    else
	    {
		    isMoving = false;
		    Anim.SetBool("IsMoving", false);
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
	    isAttacking = true;
	    attackAmount++;
	    anim.SetInteger("CurrentAttack", attackAmount);
    }
    
    public void Sprint(InputAction.CallbackContext context)
    {
	    if (!isSprinting)
	    {
		    isSprinting = true;
		    Anim.SetBool("IsSprinting", true);
		    moveSpeed = maxSprintMoveSpeed;
	    }
	    else
	    {
		    isSprinting = false;
		    Anim.SetBool("IsSprinting", false);
		    moveSpeed = maxMoveSpeed;
	    }
    }
    
    private void HealthChanged(int newHealth)
    {
	    UIManager.Instance.playerHealthUi.fillAmount = (float)newHealth / baseMaxHealth;
    }
    
    #endregion
}
