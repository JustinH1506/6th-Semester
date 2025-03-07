using System;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	private PlayerInput playerInput;
	private PlayerStateMachine playerStateMachine;

	private void Awake()
	{
		playerInput = new PlayerInput();
		playerStateMachine = GetComponent<PlayerStateMachine>();
	}

	private void OnEnable()
	{
		playerInput.Enable();

		playerInput.Player.Movement.performed += playerStateMachine.Move;
		playerInput.Player.Movement.canceled += playerStateMachine.Move;

		playerInput.Player.Sprint.performed += playerStateMachine.Sprint;
		playerInput.Player.Sprint.canceled += playerStateMachine.Sprint;

		playerInput.Player.Attack.performed += playerStateMachine.Attack;

	}

	private void OnDisable()
	{
		playerInput.Disable();
		
		playerInput.Player.Movement.performed -= playerStateMachine.Move;
		playerInput.Player.Movement.canceled -= playerStateMachine.Move;
		
		playerInput.Player.Sprint.performed -= playerStateMachine.Sprint;
		playerInput.Player.Sprint.canceled -= playerStateMachine.Sprint;
		
		playerInput.Player.Attack.performed -= playerStateMachine.Attack;

	}
}
