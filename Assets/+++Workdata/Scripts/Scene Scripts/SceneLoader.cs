using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance;

	public enum SceneStates
	{
		Manager = 0,
		Level01 = 1,
		Level02 = 2
	}

	public SceneStates sceneStates;
	
	[HideInInspector]
	public int currentScene;
	
	/// <summary>
	/// Creates the instance, looks through current scenes and changes ui based on current active scene. 
	/// </summary>
	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);

			return;
		}
		
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).buildIndex >= (int)SceneStates.Level01)
			{
				int startScene = SceneManager.GetSceneAt(i).buildIndex;
				GameManager.Instance.gameStates = GameManager.GameStates.InGame;
				sceneStates = (SceneStates)startScene;
			}
		}
		
		currentScene = (int)sceneStates; 
	}
	
	public IEnumerator LoadScene(int newScene, int timeScale, bool loadGame)
	{
		UIManager.Instance.OpenMenu(UIManager.Instance.loadingScreen, CursorLockMode.Locked, 1f);
		
		sceneStates = (SceneStates)newScene;
		
		AsyncOperation loadLevel = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

		UIManager.Instance.loadingIcon.fillAmount = 0f;
		
		while (!loadLevel.isDone)
		{
			UIManager.Instance.loadingIcon.fillAmount = Mathf.Clamp01(loadLevel.progress / .9f);
			yield return null;
		}
		
		UIManager.Instance.CloseMenu(UIManager.Instance.loadingScreen, CursorLockMode.Locked, 1f);
		
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newScene));

		currentScene = newScene;

		Time.timeScale = timeScale;

		if (loadGame)
		{
			DataPersistenceManager.Instance.LoadGame();
		}
	}

	/// <summary>
	/// Loads the given scene and unloads the old scene. 
	/// </summary>
	/// <param name="oldScene"></param>
	/// <param name="firstNewScene"></param>
	/// <param name="timeScale"></param>
	/// <returns></returns>
	public IEnumerator LoadScene(int oldScene, int firstNewScene, int timeScale)
	{
		UIManager.Instance.OpenMenu(UIManager.Instance.loadingScreen, CursorLockMode.Locked, 1f);
		
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);

			sceneStates = (SceneStates)firstNewScene;
		}
		
		AsyncOperation loadLevel = SceneManager.LoadSceneAsync(firstNewScene, LoadSceneMode.Additive);
		
		UIManager.Instance.loadingIcon.fillAmount = 0f;
		
		while (!loadLevel.isDone)
		{
			UIManager.Instance.loadingIcon.fillAmount = Mathf.Clamp01(loadLevel.progress / .9f);
			yield return null;
		}
		
		UIManager.Instance.CloseMenu(UIManager.Instance.loadingScreen, CursorLockMode.Locked, 1f);
		
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstNewScene));

		currentScene = firstNewScene;

		Time.timeScale = timeScale;
	}

	public IEnumerator UnloadScene(int oldScene, int currentActiveScene, int timeScale)
	{
		UIManager.Instance.OpenMenu(UIManager.Instance.loadingScreen, CursorLockMode.Locked, 1f);
		
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);

			sceneStates = (SceneStates)currentActiveScene;
		}
		
		UIManager.Instance.loadingIcon.fillAmount = 0f;
		
		UIManager.Instance.CloseMenu(UIManager.Instance.loadingScreen, CursorLockMode.None, 1f);
		
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentActiveScene));

		currentScene = currentActiveScene;

		Time.timeScale = timeScale;
	}
}
