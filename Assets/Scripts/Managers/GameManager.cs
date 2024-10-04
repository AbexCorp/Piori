using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        ChangeState(GameState.PrepareGame);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.PrepareGame:
                GridManager.Instance.GenerateGrid();
                PlayerController.Instance.Spawn();
                break;
            case GameState.StartPhase:
                break;
            case GameState.BattlePhase:
                break;
            case GameState.BuyPhase:
                break;
            case GameState.Ending:
                break;
            
        }
    }

    public void LoseGame()
    {
        MenuManager.Instance.ShowWarning("You Lose");
        Debug.LogWarning("Lost game");
        Debug.Break();
        Time.timeScale = 0;
    }
}

public enum GameState
{
    PrepareGame = 0,
    StartPhase = 1,
    BattlePhase = 2,
    BuyPhase = 3,
    Ending = 4,
}
