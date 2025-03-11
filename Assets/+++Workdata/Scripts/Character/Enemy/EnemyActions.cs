using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : CharacterBase
{
	private NavMeshAgent navMeshAgent;
	
	private Transform playerTransform;
	
	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void FixedUpdate()
	{
		navMeshAgent.SetDestination(playerTransform.position);
	}
}
