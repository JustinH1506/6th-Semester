using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	
	public Image loadingIcon;

	[Header("Canvas Groups")]
	public CanvasGroup loadingScreen;
	public CanvasGroup mainMenuScreen;
	public CanvasGroup gameOverScreen;
	public CanvasGroup optionsScreen;
	public CanvasGroup inGameUi;
	public CanvasGroup pauseScreen;

	[Header("Buttons")] 
	
	[Space] 
	
	[Header("Images")]

	public Image playerHealthUi;
	public Image playerStaminaUi;
	
	[Space]

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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && pauseScreen.alpha <= 1)
		{
			OpenMenu(pauseScreen, CursorLockMode.None, 0f);
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && pauseScreen.alpha >= 1)
		{
			CloseMenu(pauseScreen, CursorLockMode.None, 0f);
		}
	}

	public void StartNewGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, false));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
		DataPersistenceManager.Instance.NewGame();
	}

	public void LoadGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1, true));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
	}

	public void ReloadGame()
	{
		StartCoroutine(SceneLoader.Instance.LoadScene(SceneLoader.Instance.currentScene, (int)SceneLoader.Instance.sceneStates, 1)); 
		CloseMenu(gameOverScreen, CursorLockMode.Locked, 1f);
	}

	public void Resume()
	{
		CloseMenu(pauseScreen, CursorLockMode.Locked, 1f);
	}

	public void OpenMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale)
	{
		canvasGroup.ShowCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			//player.GetComponent<PlayerActions>().DisablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

	public void CloseMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale)
	{
		canvasGroup.HideCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			//player.GetComponent<PlayerActions>().EnablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}

	public void SaveGame()
	{
		DataPersistenceManager.Instance.SaveGame();
	}

	public void Quit()
	{
		Application.Quit();
	}
}
