using System;
using UnityEngine;

public enum GameState
{
    Lobby,
    Game,
    Pause,
    Victory,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public static Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetGameState(GameState.Lobby);
    }

    public void SetGameState(GameState nextState)
    {
        state = nextState;

        switch (nextState)
        {
            case GameState.Lobby:
                HandleLobbyGameState();
                break;
            case GameState.Game:
                HandleGameGameState();
                break;
            case GameState.Pause:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(nextState);
    }

    private void HandleLobbyGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.LobbyMenu, true);
    }

    private void HandleGameGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.LobbyMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.GameMenu, true);


    }
}
