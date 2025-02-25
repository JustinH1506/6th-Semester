using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance;

	public enum SceneStates
	{
		Manager = 0,
		MainMenu = 1,
		Level01 = 2,
		Level02 = 3
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

	/// <summary>
	/// Loads the given scene and unloads the old scene. 
	/// </summary>
	/// <param name="oldScene"></param>
	/// <param name="firstNewScene"></param>
	/// <param name="timeScale"></param>
	/// <returns></returns>
	public IEnumerator LoadScene(int oldScene, int firstNewScene, int timeScale)
	{
		UIManager.Instance.loadingScreen.SetActive(true);
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);

			sceneStates = (SceneStates)firstNewScene;
		}
		
		AsyncOperation loadLevel = SceneManager.LoadSceneAsync(firstNewScene, LoadSceneMode.Additive);
		
		while (!loadLevel.isDone)
		{
			UIManager.Instance.loadingSlider.fillAmount = Mathf.Clamp01(loadLevel.progress / .9f);
			yield return null;
		}
		
		UIManager.Instance.loadingScreen.SetActive(false);
		
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstNewScene));

		currentScene = firstNewScene;

		Time.timeScale = timeScale;
	}
    
	/// <summary>
	/// Unloads the given scene and loads 2 new given scenes.
	/// </summary>
	/// <param name="oldScene"></param>
	/// <param name="firstNewScene"></param>
	/// <param name="secondNewScene"></param>
	/// <param name="timeScale"></param>
	/// <returns></returns>
	public IEnumerator LoadScene(int oldScene, int firstNewScene , int secondNewScene, int timeScale)
	{
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);

			sceneStates = (SceneStates)firstNewScene;
		}

		SceneManager.LoadScene(firstNewScene, LoadSceneMode.Additive);
		SceneManager.LoadScene(secondNewScene, LoadSceneMode.Additive);

		currentScene = firstNewScene;

		Time.timeScale = timeScale;
	}
}
