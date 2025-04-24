using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	public event System.Action<int> OnHealthChanged;

	[Header("Base Variables")]
	[SerializeField] protected int baseMaxHealth;
	[SerializeField] protected int baseCurrentHealth;
	[SerializeField] protected int baseDefense;
	public int baseAttack;
	[Space]
	
	protected bool isDead = false;

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
			if (gameObject.CompareTag("Player"))
			{
				UIManager.Instance.OpenMenu(UIManager.Instance.gameOverScreen, CursorLockMode.None, 0f);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
	
	protected virtual void Awake()
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
