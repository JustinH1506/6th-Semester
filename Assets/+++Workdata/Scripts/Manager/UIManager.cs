using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	public GameObject loadingScreen;

	public Image loadingSlider;

	public GameObject mainMenuScreen;

	private GameObject player;

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

	public void OpenMenu(GameObject menu, CursorLockMode lockMode, float timeScale)
	{
		menu.SetActive(true);

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			player.GetComponent<PlayerMovement>().DisablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

	public void CloseMenu(GameObject menu, CursorLockMode lockMode, float timeScale)
	{

		menu.SetActive(false);

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			player.GetComponent<PlayerMovement>().EnablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}
}
