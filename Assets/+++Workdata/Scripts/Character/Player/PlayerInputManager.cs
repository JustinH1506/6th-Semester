using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	private PlayerInput playerInput;
	private PlayerActions _playerActions;

	private void Awake()
	{
		playerInput = new PlayerInput();
		_playerActions = GetComponent<PlayerActions>();
	}

	private void OnEnable()
	{
		playerInput.Enable();

		playerInput.Player.Movement.performed += _playerActions.Move;
		playerInput.Player.Movement.canceled += _playerActions.Move;

		playerInput.Player.Sprint.performed += _playerActions.Sprint;
		playerInput.Player.Sprint.canceled += _playerActions.Sprint;

		playerInput.Player.Jump.performed += _playerActions.Jump;
		playerInput.Player.Jump.canceled += _playerActions.Jump;

		playerInput.Player.Attack.performed += _playerActions.Attack;

	}

	private void OnDisable()
	{
		playerInput.Disable();
		
		playerInput.Player.Movement.performed -= _playerActions.Move;
		playerInput.Player.Movement.canceled -= _playerActions.Move;
		
		playerInput.Player.Sprint.performed -= _playerActions.Sprint;
		playerInput.Player.Sprint.canceled -= _playerActions.Sprint;
		
		playerInput.Player.Jump.performed -= _playerActions.Jump;
		playerInput.Player.Jump.canceled -= _playerActions.Jump;
		
		playerInput.Player.Attack.performed -= _playerActions.Attack;

	}
}
