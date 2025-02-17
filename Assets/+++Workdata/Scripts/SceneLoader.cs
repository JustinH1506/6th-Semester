using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance;
	
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
		}
	}

	/// <summary>
	/// Loads the given scene and unloads the old scene. 
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadScene(int oldScene, int firstNewScene, int timeScale)
	{
		UIManager.Instance.loadingScreen.SetActive(true);
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);
		}
		
		AsyncOperation loadLevel = SceneManager.LoadSceneAsync(firstNewScene, LoadSceneMode.Additive);
		
		while (!loadLevel.isDone)
		{
			yield return null;
		}
		
		UIManager.Instance.loadingScreen.SetActive(false);
		
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstNewScene));

		Time.timeScale = timeScale;
	}
    
	/// <summary>
	/// Unloads the given scene and loads 2 new given scenes.
	/// </summary>
	/// <returns></returns>
	public IEnumerator LoadScene(int oldScene, int firstNewScene , int secondNewScene, int timeScale)
	{
		var unloadedScene = SceneManager.GetSceneByBuildIndex(oldScene);
		
		if (unloadedScene.isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(unloadedScene);
		}

		SceneManager.LoadScene(firstNewScene, LoadSceneMode.Additive);
		SceneManager.LoadScene(secondNewScene, LoadSceneMode.Additive);

		Time.timeScale = timeScale;
	}
}
