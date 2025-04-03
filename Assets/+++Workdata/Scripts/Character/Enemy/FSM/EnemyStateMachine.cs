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
	
	public EnemyBaseState CurrentState { get => currentState;
		set => currentState = value;
	}
	
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

	private Data data;
	
	#endregion
	
	#region Getters and Setters
	
	public NavMeshAgent NavMeshAgent { get => navMeshAgent;
		set => navMeshAgent = value;
	}
	
	public Transform PlayerTransform { get => playerTransform;
		set => playerTransform = value;
	}
	
	public Animator Anim => anim;

	public Transform[] CheckPoints => checkPoints;

	public int CurrentPoint { get => currentPoint;
		set => currentPoint = value;
	}
	
	public float DistanceThreshold => distanceThreshold;

	public bool GotHit { get => gotHit;
		set => gotHit = value;
	}
	
	#endregion

	#region Data Class
	
	[System.Serializable]
	public class Data
	{
		public Vector3 position;
		public int currentPatrolPoint;
		public bool isDead;
	}
	
	#endregion
	
	#region Methods
	
	private void Awake()
	{
		data = new Data();
		
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
		data.position = transform.position;
		data.currentPatrolPoint = CurrentPoint;
		data.isDead = isDead;
		gameData.enemyPositionByGuid.Add(uniqueGuid, data);
	}

	public void LoadData(GameData gameData)
	{
		if (gameData.enemyPositionByGuid.TryGetValue(uniqueGuid, out data))
		{
			data = gameData.GetEnemyPosition(uniqueGuid);
			
			if (data.isDead)
			{
				gameObject.SetActive(false);
				return;
			}
			
			transform.position = data.position;
			
			CurrentPoint = data.currentPatrolPoint;
			
			NavMeshAgent.destination = CheckPoints[CurrentPoint].position;
		}
	}
	
	#endregion
}
