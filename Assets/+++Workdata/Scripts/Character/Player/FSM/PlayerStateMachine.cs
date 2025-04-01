using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : CharacterBase, IDataPersistence
{
	public event Action<float> OnStaminaChanged;
	
    #region States
    
    private PlayerBaseState currentState;
    public PlayerStateFactory states;
    
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    
    #endregion
    
    #region Movement Variabels
    
    [Header("Movement Variables"), Tooltip("Max movement speed during walking.")]
    [SerializeField] private float maxMoveSpeed = 0;
    [SerializeField] private float maxSprintMoveSpeed = 0;
    [SerializeField] private float rotationSpeed = 0f;
    private float moveSpeed = 0;
    private bool isSprinting = false;
    private bool canTurn = true;
    
    [Space]
    #endregion

    #region Stamina

    private float currentStamina = 0f;
    private float runCost = 5f;
    [SerializeField] private float maxStamina = 50f;
    
    #endregion
    
    #region Attack Variables

    private int attackAmount = 0;
    private bool isAttacking = false;
    private bool isDodging = false;

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
    public float Stamina {get { return currentStamina; } set{ SetCurrentStamina(value); } }
    public float RunCost {get { return runCost; } }
    public bool IsSprinting {get { return isSprinting; } }
    public bool IsMoving {get { return isMoving; } }
    public bool CanTurn {get { return canTurn; } set { canTurn = value; } }
    
    #endregion

    #region Attack

    public int AttackAmount {get { return attackAmount; } set { attackAmount = value; } }
    public bool IsAttacking {get { return isAttacking; }  set { isAttacking = value; } }
    public bool IsDodging {get { return isDodging; }  set { isAttacking = value; } }
    
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
	    currentStamina = maxStamina;
    }

    private void Start()
    {
	    moveSpeed = maxMoveSpeed;
    }
    
    private void OnEnable()
    {
	    OnRegisterCurrentHealth(HealthChanged, true);
	    OnRegisterCurrentStamina(StaminaChanged, true);
    }
    private void OnDisable()
    {
	    OnHealthChanged -= HealthChanged;
	    OnStaminaChanged -= StaminaChanged;
    }

    private void FixedUpdate()
    {
	    currentState.UpdateState();
    }

    public void LoadData(GameData data)
    {
	    transform.position = data.playerPosition;
	    CurrentHealth = data.playerHp;
    }

    public void SaveData(GameData data)
    {
	    data.playerPosition = transform.position;
	    data.playerHp = CurrentHealth;
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
	    Vector3 cameraRelativeMovement =  HandleCameraRelative();
	    
	    HandleRotation(cameraRelativeMovement, rotationSpeed);
		
	    rb.linearVelocity = new Vector3(cameraRelativeMovement.x * moveSpeed, rb.linearVelocity.y, cameraRelativeMovement.z * moveSpeed);
    }

    public void HandleRotation(Vector3 cameraRelativeMovement, float rotateSpeed)
    {
	    if (HandleCameraRelative() != Vector3.zero)
	    {
		    transform.forward = Vector3.Slerp(transform.forward, cameraRelativeMovement.normalized, Time.deltaTime * rotateSpeed);
	    }
    }

    public Vector3 HandleCameraRelative()
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
	    
	    return cameraRelativeMovement;
    }

    public void Attack(InputAction.CallbackContext context)
    {
	    if (currentStamina > 0.05f)
	    {
		    anim.SetBool("IsAttacking", true);
		    isAttacking = true;
		    attackAmount++;
		    anim.SetInteger("CurrentAttack", attackAmount);
	    }
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

    public void Dodge(InputAction.CallbackContext context)
    {
	    isDodging = true;
    }

    public void GetCurrentStamina()
    {
	    if (currentStamina < 50f)
	    {
			Stamina += Time.deltaTime * runCost;
	    }
    }

    public void SetCurrentStamina(float newStamina)
    {
	    if (newStamina > maxStamina)
		    newStamina = maxStamina;
	    
	    currentStamina = newStamina;

	    if (OnStaminaChanged != null)
	    {
		    OnStaminaChanged(currentStamina);
	    }
    }

    public void AttackStaminaUse()
    {
	    Stamina -= runCost;
    }
    
    private void HealthChanged(int newHealth)
    {
	    UIManager.Instance.playerHealthUi.fillAmount = (float)newHealth / baseMaxHealth;
    }

    private void StaminaChanged(float newStamina)
    {
	    UIManager.Instance.playerStaminaUi.fillAmount = newStamina / maxStamina;
    }

    private void OnRegisterCurrentStamina(Action<float> callback, bool getInstantCallback = false)
    {
	    OnStaminaChanged += callback;
	    if (getInstantCallback)
		    callback(currentStamina);
    }

    public void ChangeTurnState()
    {
	    canTurn = !canTurn;
    }

    public void ChangeTag()
    {
	    if (!CompareTag("Invulnerable"))
	    {
			tag = "Invulnerable";
	    }
	    else
	    {
		    tag = "Player";
	    }
    }
    
    #endregion
}
