using UnityEngine;

public enum Menu
{
    LobbyMenu,
    GameMenu,
    VictoryMenu,
    LoseMenu
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private GameObject lobbyMenu;
    private GameObject gameMenu;
    private GameObject victoryMenu;
    private GameObject loseMenu;

    void Awake()
    {
        Instance = this;

        GameObject canvas = GameObject.Find("Canvas");
        lobbyMenu = canvas.transform.Find("LobbyMenu").gameObject;
        gameMenu = canvas.transform.Find("GameMenu").gameObject;
        victoryMenu = canvas.transform.Find("VictoryMenu").gameObject;
        loseMenu = canvas.transform.Find("LoseMenu").gameObject;
    }

    public void ToggleMenu(Menu menu, bool value)
    {
        switch (menu)
        {
            case Menu.LobbyMenu:
                lobbyMenu.SetActive(value);
                break;
            case Menu.GameMenu:
                gameMenu.SetActive(value);
                break;
            case Menu.VictoryMenu:
                victoryMenu.SetActive(value);
                break;
            case Menu.LoseMenu:
                loseMenu.SetActive(value);
                break;
            default:
                break;
        }
    }
}
