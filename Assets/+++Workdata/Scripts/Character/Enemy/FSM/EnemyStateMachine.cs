using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : CharacterBase, IDataPersistence
{
	#region States
	
	private EnemyBaseState currentState;
	private EnemyStateFactory states;
	
	public EnemyBaseState CurrentState { get { return currentState; } set { currentState = value; } }
	
	#endregion
	
	#region Variables

	[SerializeField] private string uniqueGuid;
	
	private NavMeshAgent navMeshAgent;
	
	private Transform playerTransform;

	private Animator anim;
	
	[SerializeField] private float distanceThreshold = 0f;

	[SerializeField] private Transform[] checkPoints;
	
	private int currentPoint = 0;
	
	private bool gotHit = false;
	
	#endregion
	
	#region Getters and Setters
	
	public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } set { navMeshAgent = value; } }
	
	public Transform PlayerTransform { get { return playerTransform; } set { playerTransform = value; } }
	
	public Animator Anim { get { return anim; } }
	
	public Transform[] CheckPoints { get { return checkPoints; } }
	
	public int CurrentPoint { get { return currentPoint; } set { currentPoint = value; } }
	
	public float DistanceThreshold { get { return distanceThreshold; } }
	
	public bool GotHit { get { return gotHit; } set { gotHit = value; } }
	
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
	
	private void OnValidate()
	{
		if (string.IsNullOrEmpty(gameObject.scene.name))
		{
			uniqueGuid = "";
		}
		else if (string.IsNullOrEmpty(uniqueGuid))
		{
			uniqueGuid = System.Guid.NewGuid().ToString();
		}
	}

	public float DistanceBetweenPlayer()
	{
		return Vector3.Distance(transform.position, PlayerTransform.position);
	}

	public void SaveData(GameData gameData)
	{
		if (gameData.enemyPositionByGuid.ContainsKey(uniqueGuid))
		{
			gameData.enemyPositionByGuid.Remove(uniqueGuid);
		}
		gameData.enemyPositionByGuid.Add(uniqueGuid, transform.position);
	}

	public void LoadData(GameData data)
	{
		if (data.enemyPositionByGuid.TryGetValue(uniqueGuid, out Vector3 position))
		{
			transform.position = data.GetEnemyPosition(uniqueGuid);
		}
	}
}
