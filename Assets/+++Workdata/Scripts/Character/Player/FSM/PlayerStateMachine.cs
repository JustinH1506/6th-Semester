using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : CharacterBase, IDataPersistence
{
	#region Actions
	public event Action<float> onStaminaChanged;
	
	#endregion
	
    #region States
    
    private PlayerBaseState currentState;
    private PlayerStateFactory states;
    
    public PlayerBaseState CurrentState { get => currentState;
	    set => currentState = value;
    }
    
    #endregion
    
    #region Camera
    
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    
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
    
    [Header("Stamina Variables"), Tooltip("Variables for the Stamina System.")]
    [SerializeField] private float staminaRecovery = 5f;
    [SerializeField] private float maxStamina = 50f;
    private float currentStamina = 0f;
    private float runCost = 5f;
    
    [Space]
    
    #endregion
    
    #region Attack Variables

    private int attackAmount = 0;
    private bool isAttacking = false;
    private bool isDodging = false;

    #endregion
	
    #region Animations
    [Header("Animations")]
    private Animator anim;
    public PlayerAnimationFactory anims;
    
    #endregion
    
    #region Getters and Setters

    #region Movement
    
    public float MaxMoveSpeed {get => maxMoveSpeed;
	    set => maxMoveSpeed = value;
    }
    public float MaxSprintMoveSpeed {get => maxSprintMoveSpeed;
	    set => maxSprintMoveSpeed = value;
    }
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float Stamina {get => currentStamina;
	    set => SetCurrentStamina(value);
    }
    public float RunCost => runCost;
    public bool IsSprinting => isSprinting;
    public bool IsMoving => isMoving;
    public bool CanTurn {get => canTurn;
	    set => canTurn = value;
    }
    
    public float InputZ => inputZ;
    public float InputX => inputX;
    
    #endregion

    #region Attack

    public int AttackAmount 
    {
	    get => attackAmount;
	    set => attackAmount = value;
    }
    public bool IsAttacking 
    {
	    get => isAttacking;
	    set => isAttacking = value;
    }

    public bool IsDodging
    {
	    get => isDodging;
	    set => isDodging = value;
    }

    #endregion
    
    #region Stuff
    
    public Rigidbody Rb => rb;

    #endregion

    #region Animator

    public Animator Anim => anim;

    #endregion
    
    [Space]
    
    #endregion
    
    #region Stuff
    
    private Rigidbody rb;
    private float inputX;
    private float inputZ;
    private bool isMoving;
    
	#endregion
	
	#region Methods
	
	#region Unity Methods
	
	/// <summary>
	/// Sets everything up and gets us in the Idle state at the start
	/// </summary>
    protected override void Awake()
    {
	    base.Awake();
	    states = new PlayerStateFactory(this);
	    anims = new PlayerAnimationFactory();
	    anim  = GetComponent<Animator>();
	    currentState = states.Idle();
	    currentState.EnterState();
	    rb = GetComponent<Rigidbody>();
	    currentStamina = maxStamina;
    }

    private void Start()
    {
	    moveSpeed = maxMoveSpeed;
    }
    
    /// <summary>
    /// When Enabled enables Health and Stamina Action
    /// </summary>
    private void OnEnable()
    {
	    OnRegisterCurrentHealth(HealthChanged, true);
	    OnRegisterCurrentStamina(StaminaChanged, true);
    }
    
    /// <summary>
    /// When Disabled disables Health and Stamina Action
    /// </summary>
    private void OnDisable()
    {
	    OnHealthChanged -= HealthChanged;
	    onStaminaChanged -= StaminaChanged;
    }

    /// <summary>
    /// Calls the UpdateState from the Current state
    /// </summary>
    private void Update()
    {
	    currentState.UpdateState();
    }

    private void FixedUpdate()
    {
	    currentState.FixedUpdateState();
    }

    #endregion

    #region My Methods
    
    /// <summary>
    /// Reads the input and sets the moving animation
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// Handles how the Character moves.
    /// </summary>
    public void HandleMovement()
    {
	    Vector3 cameraRelativeMovement =  HandleCameraRelative();
	    
	    HandleRotation(cameraRelativeMovement, rotationSpeed);

	    Vector3 movement = new Vector3(cameraRelativeMovement.x * moveSpeed, rb.linearVelocity.y, cameraRelativeMovement.z * moveSpeed);
		
	    rb.linearVelocity = movement;
    }

    /// <summary>
    /// Changes the characters Rotation depending on Input.
    /// </summary>
    /// <param name="cameraRelativeMovement"></param>
    /// <param name="rotateSpeed"></param>
    public void HandleRotation(Vector3 cameraRelativeMovement, float rotateSpeed)
    {
	    if (HandleCameraRelative() != Vector3.zero)
	    {
		    transform.forward = Vector3.Slerp(transform.forward, cameraRelativeMovement.normalized, Time.deltaTime * rotateSpeed);
	    }
    }
    
    public void HandleDodge()
    {
	    Vector3 cameraRelativeMovement =  HandleCameraRelative();

	    Vector3 dodgeDirection; 
	    
	    if (cameraRelativeMovement != Vector3.zero)
	    {
		    dodgeDirection = new Vector3(cameraRelativeMovement.x * 3, rb.linearVelocity.y, cameraRelativeMovement.z * 3);
	    }
	    else
	    {
		    dodgeDirection = transform.forward * 3;
	    }
	    
	    rb.AddForce(dodgeDirection * runCost, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Handles the movement based on the cameras forward. 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Starts the Attack when the Player has enough stamina. 
    /// </summary>
    /// <param name="context"></param>
    public void Attack(InputAction.CallbackContext context)
    {
	    if (currentStamina > 0.05f)
	    {
		    isAttacking = true;
		    attackAmount++;
	    }
    }
    
    /// <summary>
    /// Lets the Player sprint. 
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// Lets the Player dodge. 
    /// </summary>
    /// <param name="context"></param>
    public void Dodge(InputAction.CallbackContext context)
    {
	    if (context.performed)
	    {
			isDodging = true;
	    }
    }

    /// <summary>
    /// Recovers Stamina over time. 
    /// </summary>
    public void GetCurrentStamina()
    {
	    if (currentStamina < 50f)
	    {
			Stamina += Time.deltaTime * staminaRecovery;
	    }
    }

    /// <summary>
    /// Sets the stamina when the value got changed. 
    /// </summary>
    /// <param name="newStamina"></param>
    private void SetCurrentStamina(float newStamina)
    {
	    if (newStamina > maxStamina)
		    newStamina = maxStamina;
	    
	    currentStamina = newStamina;

	    if (onStaminaChanged != null)
	    {
		    onStaminaChanged(currentStamina);
	    }
    }

    /// <summary>
    /// Amount of Stamina used when attacking. 
    /// </summary>
    public void AttackStaminaUse()
    {
	    Stamina -= runCost;
    }
    
    /// <summary>
    /// Changed the Player health ui based on the current player health.
    /// </summary>
    /// <param name="newHealth"></param>
    private void HealthChanged(int newHealth)
    {
	    UIManager.Instance.playerHealthUi.fillAmount = (float)newHealth / baseMaxHealth;
    }

    /// <summary>
    /// Changes the Player Stamina ui based on the current stamina
    /// </summary>
    /// <param name="newStamina"></param>
    private void StaminaChanged(float newStamina)
    {
	    UIManager.Instance.playerStaminaUi.fillAmount = newStamina / maxStamina;
    }

    /// <summary>
    /// Subscribes onStaminaChanged to callback.
    /// calls currentStamina method.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="getInstantCallback"></param>
    private void OnRegisterCurrentStamina(Action<float> callback, bool getInstantCallback = false)
    {
	    onStaminaChanged += callback;
	    if (getInstantCallback)
		    callback(currentStamina);
    }

    /// <summary>
    /// changes canTurn bool to the opposite of what it curretnly is. 
    /// </summary>
    public void ChangeTurnState()
    {
	    canTurn = !canTurn;
    }

    /// <summary>
    /// Makes the player invulnerable. 
    /// </summary>
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

    #region Save Methods
    
    /// <summary>
    /// Loads data from save file. 
    /// </summary>
    /// <param name="gameData"></param>
    public void LoadData(GameData gameData)
    {
	    transform.position = gameData.playerPosition;
	    CurrentHealth = gameData.playerHp;
	    
	    cinemachineFreeLook.ForceCameraPosition(gameData.cameraPosition, gameData.cameraRotation);
    }

    /// <summary>
    /// Save data into save file. 
    /// </summary>
    /// <param name="gameData"></param>
    public void SaveData(GameData gameData)
    {
	    gameData.playerPosition = transform.position;
	    gameData.playerHp = CurrentHealth;
	    
	    gameData.cameraPosition = cinemachineFreeLook.transform.position;
	    gameData.cameraRotation = cinemachineFreeLook.transform.rotation;
    }
    
    #endregion
    
    #endregion
}