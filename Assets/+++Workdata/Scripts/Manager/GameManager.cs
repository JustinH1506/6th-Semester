using System;
using UnityEngine;

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
    }
}
