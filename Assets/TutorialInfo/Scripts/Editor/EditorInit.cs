using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class EditorInit
{
    /// <summary>
    /// Saves the last active scene and Loads the Manager Scene when starting Play Mode.
    /// </summary>
    static EditorInit()
    {
        EditorPrefs.SetString("activeScene" ,SceneManager.GetActiveScene().name);
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
