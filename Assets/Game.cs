using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{       
    private GameState _gameState;
    public static event GameAction GameStartedEvent;
    public delegate void GameAction();
    private static Game _instance;
    
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        SetGameState(GameState.RUNNING);

        if (GameStartedEvent != null)
            GameStartedEvent();
    }

    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    public static Game GetInstance()
    {
        return _instance;
    }
}
