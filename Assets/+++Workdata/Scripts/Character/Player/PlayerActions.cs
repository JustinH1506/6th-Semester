using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : CharacterBase
{
	[Header("Movement Variables")]
	[SerializeField] private float moveSpeed = 0;
	[SerializeField] private float jumpStrength = 0;
	[SerializeField] private float jumpDetectorMaxLength = 0;
	[SerializeField] private float rotationSpeed = 0f;
	[SerializeField] private LayerMask groundMask = 0;
	[Space] 
	
	[Header("Animations")] 
	private Animator anim;
	
	private Rigidbody rb;
	private float inputX;
	private float inputZ;
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
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
}
