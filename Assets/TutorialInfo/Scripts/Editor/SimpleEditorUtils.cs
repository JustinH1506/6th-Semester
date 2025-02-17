using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class SimpleEditorUtils
{
    [MenuItem("Edit/Play-Unplay, But From Prelaunch Scene %0")]
    public static void PlayFromPrelaunchScene()
    {
        if ( EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/+++Workdata/Scenes/Manager.unity");
        EditorApplication.isPlaying = true;
    }
}
