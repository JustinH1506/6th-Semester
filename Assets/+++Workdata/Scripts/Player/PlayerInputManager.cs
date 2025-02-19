using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	private PlayerInput playerInput;
	private PlayerMovement playerMovement;

	private void Awake()
	{
		playerInput = new PlayerInput();
		playerMovement = GetComponent<PlayerMovement>();
	}

	private void OnEnable()
	{
		playerInput.Enable();

		playerInput.Player.Movement.performed += playerMovement.Move;
		playerInput.Player.Movement.canceled += playerMovement.Move;

		playerInput.Player.Jump.performed += playerMovement.Jump;
		playerInput.Player.Jump.canceled += playerMovement.Jump;
	}

	private void OnDisable()
	{
		playerInput.Disable();
		
		playerInput.Player.Movement.performed -= playerMovement.Move;
		playerInput.Player.Movement.canceled -= playerMovement.Move;
		
		playerInput.Player.Jump.performed -= playerMovement.Jump;
		playerInput.Player.Jump.canceled -= playerMovement.Jump;
	}
}
