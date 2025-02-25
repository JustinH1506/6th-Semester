using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,
        InGame
    }
    
    public static GameManager Instance;

    public GameStates gameStates;
    
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
        
        #if UNITY_EDITOR

        string activeSceneName = EditorPrefs.GetString("activeScene");
        
        if (activeSceneName != null)
        {
            SceneManager.LoadScene(activeSceneName, LoadSceneMode.Additive);
        }
        
        #endif
    }
}
