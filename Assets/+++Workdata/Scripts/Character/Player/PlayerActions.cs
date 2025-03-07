using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : CharacterBase
{
	[Header("Movement Variables"), Tooltip("Max movement speed during walking.")] 
	[SerializeField] private float maxMoveSpeed = 0;
	[SerializeField] private float maxSprintMoveSpeed = 0;
	private float moveSpeed = 0;
	private bool isSprinting = false;
	[Space]
	
	[SerializeField] private float jumpStrength = 0;
	[SerializeField] private float jumpDetectorMaxLength = 0;
	[SerializeField] private float rotationSpeed = 0f;
	[SerializeField] private LayerMask groundMask = 0;
	[Space] 
	
	[Header("Animations")] 
	private Animator anim;
	[Space]
	
	private Rigidbody rb;
	private float inputX;
	private float inputZ;
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		
		moveSpeed = maxMoveSpeed;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnEnable()
	{
		OnRegisterCurrentHealth(HealthChanged, true);
	}
	private void OnDisable()
	{
		OnHealthChanged -= HealthChanged;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			CurrentHealth--;
		}
	}

	private void FixedUpdate()
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

	public void Move(InputAction.CallbackContext context)
	{
		inputX = context.ReadValue<Vector3>().x;
		inputZ = context.ReadValue<Vector3>().z;
	}

	public void Attack(InputAction.CallbackContext context)
	{
		anim.SetTrigger("Attack");
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (IsGrounded())
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpStrength, rb.linearVelocity.z);
		}
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

	private bool IsGrounded()
	{
		if (Physics.Raycast(transform.position, Vector3.down, jumpDetectorMaxLength, groundMask))
		{
			return true;
		}

		return false;
	}
	
	public void EnablePlayerActions()
	{
		
	}

	public void DisablePlayerActions()
	{
		
	}
	
	private void HealthChanged(int newHealth)
	{
		UIManager.Instance.playerHealthUi.fillAmount = (float)newHealth / baseMaxHealth;
		Debug.Log((float)newHealth / baseMaxHealth);
	}
}
