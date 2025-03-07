using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorInit
{
    /// <summary>
    /// Always starts the game with the Manager scene.
    /// </summary>
    static EditorInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
