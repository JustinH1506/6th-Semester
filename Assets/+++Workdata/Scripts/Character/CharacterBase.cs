using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	public event System.Action<int> OnHealthChanged;

	[SerializeField] protected int baseMaxHealth;
	[SerializeField] protected int baseCurrentHealth;
	[SerializeField] protected int baseDefense;
	public int baseAttack;

	public int CurrentHealth
	{
		get => baseCurrentHealth;
	
		set => SetCurrentHealth(value);
	}
	
	private void SetCurrentHealth(int newHealth)
	{
		if (newHealth > baseMaxHealth)
			newHealth = baseMaxHealth;
	
		baseCurrentHealth = newHealth;
	
		if (OnHealthChanged != null)
		{
			OnHealthChanged(baseCurrentHealth);
		}
		
		if (baseCurrentHealth <= 0)
		{
			gameObject.SetActive(false);
		}
	}
	
	private void Awake()
	{
		SetCurrentHealth(baseMaxHealth);
	}

	public void TakeDamage(int damage)
	{
		CurrentHealth -= (damage - baseDefense);
	}
	
	public void OnRegisterCurrentHealth(System.Action<int> callback, bool getInstantCallback = false)
	{
		OnHealthChanged += callback;
		if (getInstantCallback)
			callback(baseCurrentHealth);
	}
	
}
