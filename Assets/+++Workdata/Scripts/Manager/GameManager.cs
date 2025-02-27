using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,
        InGame
    }
    
    [Header("Game Variables")]
    
    public static GameManager Instance;
    
    public GameStates gameStates;
    [Space]
    
    [Header("Debug variables.")]
    [SerializeField] public Debug _debug;
    
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

        if (_debug.useEditorCode)
        {
            string activeSceneName = _debug.startScene.name;
            
            if (activeSceneName != String.Empty)
            {
                SceneManager.LoadScene(activeSceneName, LoadSceneMode.Additive);
            }
        }
        else
        {
            UIManager.Instance.OpenMenu(UIManager.Instance.mainMenuScreen, CursorLockMode.None, 1f);
        }
        
        #endif
    }
}
