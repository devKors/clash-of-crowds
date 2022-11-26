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
        // Application.targetFrameRate = 60;
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
                HandleVictoryGameState();
                break;
            case GameState.Lose:
                HandleLoseGameState();
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(nextState);
    }

    private void HandleLobbyGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.LoseMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.VictoryMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.LobbyMenu, true);
        LevelManager.Instance.GenerateLevel();
    }

    private void HandleGameGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.LobbyMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.GameMenu, true);
    }

    private void HandleLoseGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.GameMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.LoseMenu, true, 1.5f);

        int moneyMultiplyer = PlayerPrefs.GetInt(SerializableFields.MoneyMultiplyer, 0);
        int coins = PlayerPrefs.GetInt(SerializableFields.Coins, 0);
        int newCoins = coins + GlobalConstants.PermanentLoseReward + (GlobalConstants.AdditionalLoseReward * moneyMultiplyer);
        PlayerPrefs.SetInt(SerializableFields.Coins, newCoins);
    }

    private void HandleVictoryGameState()
    {
        MenuManager.Instance.ToggleMenu(Menu.GameMenu, false);
        MenuManager.Instance.ToggleMenu(Menu.VictoryMenu, true, 1.5f);

        int level = PlayerPrefs.GetInt(SerializableFields.Level, 1);
        PlayerPrefs.SetInt(SerializableFields.Level, level + 1);

        int moneyMultiplyer = PlayerPrefs.GetInt(SerializableFields.MoneyMultiplyer, 0);
        int coins = PlayerPrefs.GetInt(SerializableFields.Coins, 0);
        int newCoins = coins + GlobalConstants.PermanentVictoryReward + (GlobalConstants.AdditionalVictoryReward * moneyMultiplyer);
        PlayerPrefs.SetInt(SerializableFields.Coins, newCoins);
    }
}
