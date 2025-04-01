using System;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
	[SerializeField] private EnemyStateMachine _enemyActions;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !other.GetComponent<PlayerStateMachine>().IsDodging)
		{
			other.GetComponent<CharacterBase>().TakeDamage(_enemyActions.baseAttack);
			other.GetComponent<PlayerStateMachine>().Anim.SetTrigger("GotHit");
		}
	}
}
