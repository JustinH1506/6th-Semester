using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : CharacterBase
{
	#region States
	
	private EnemyBaseState currentState;
	private EnemyStateFactory states;
	
	public EnemyBaseState CurrentState { get { return currentState; } set { currentState = value; } }
	
	#endregion
	
	private NavMeshAgent navMeshAgent;
	
	private Transform playerTransform;

	private Animator anim;

	[SerializeField] private Transform[] checkPoints;
	
	private int currentPoint = 0;
	
	[SerializeField] private float distanceThreshold = 0f;
	
	#region Getters and Setters
	
	public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } set { navMeshAgent = value; } }
	
	public Transform PlayerTransform { get { return playerTransform; } set { playerTransform = value; } }
	
	public Animator Anim { get { return anim; } }
	
	public Transform[] CheckPoints { get { return checkPoints; } }
	
	public int CurrentPoint { get { return currentPoint; } set { currentPoint = value; } }
	
	public float DistanceThreshold { get { return distanceThreshold; } }
	
	#endregion
	
	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
		
		states = new EnemyStateFactory(this);
		currentState = states.Patrol();
		currentState.EnterState();
	}

	private void Update()
	{
		currentState.UpdateState();
	}

	public float DistanceBetweenPlayer()
	{
		return Vector3.Distance(transform.position, PlayerTransform.position);
	}
}
