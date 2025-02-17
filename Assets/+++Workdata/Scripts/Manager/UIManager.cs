using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	public GameObject loadingScreen;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
}
