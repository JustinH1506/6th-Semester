using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	
	public Image loadingIcon;

	[Header("Canvas Groups")]
	public CanvasGroup loadingScreen;
	public CanvasGroup mainMenuScreen;
	public CanvasGroup optionsScreen;
	public CanvasGroup inGameUi;

	[Header("Buttons")] 
	
	[Space] 
	
	[Header("Images")]

	public Image playerHealthUi;
	
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

	public void StartGame()
	{
		SceneLoader.Instance.sceneStates = SceneLoader.SceneStates.Level01;
		StartCoroutine(SceneLoader.Instance.LoadScene((int)SceneLoader.Instance.sceneStates, 1));
		CloseMenu(mainMenuScreen, CursorLockMode.Locked, 1);
		OpenMenu(inGameUi, CursorLockMode.Locked, 1f);
	}

	public void OpenMenu(CanvasGroup canvasGroup, CursorLockMode lockMode, float timeScale)
	{
		canvasGroup.ShowCanvasGroup();

		player = GameObject.FindGameObjectWithTag("Player");

		if (player != null)
		{
			player.GetComponent<PlayerActions>().DisablePlayerActions();
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
			player.GetComponent<PlayerActions>().EnablePlayerActions();
		}

		Cursor.lockState = lockMode;

		Time.timeScale = timeScale;
	}
}
