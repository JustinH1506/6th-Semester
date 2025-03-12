using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : CharacterBase
{
	private NavMeshAgent navMeshAgent;
	
	private Transform playerTransform;

	private Animator anim;
	
	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		navMeshAgent.SetDestination(playerTransform.position);

		anim.SetFloat("Distance", Vector3.Distance(transform.position, playerTransform.position));
	}
}
