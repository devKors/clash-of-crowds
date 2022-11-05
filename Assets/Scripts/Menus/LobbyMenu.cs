using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyMenu : MonoBehaviour
{
    public TextMeshProUGUI levelLabel;
    public TextMeshProUGUI coinsLabel;

    public Button towerHealthButton;
    public TextMeshProUGUI towerHealthLabelLevel;
    public TextMeshProUGUI towerHealthLabelPrice;
    public TextMeshProUGUI towerHealthLabel;
    public Image towerHealthCoin;

    public Button numberOfItemsButton;
    public TextMeshProUGUI numberOfItemsLabelLevel;
    public TextMeshProUGUI numberOfItemsLabelPrice;
    public TextMeshProUGUI numberOfItemsLabel;
    public Image numberOfItemsCoin;

    public Button moneyMultiplyerButton;
    public TextMeshProUGUI moneyMultiplyerLabelLevel;
    public TextMeshProUGUI moneyMultiplyerLabelPrice;
    public TextMeshProUGUI moneyMultiplyerLabel;
    public Image moneyMultiplyerCoin;
    

    void OnEnable()
    {
        // PlayerPrefs.DeleteAll();

        int level = PlayerPrefs.GetInt(SerializableFields.Level, 1);
        int coins = PlayerPrefs.GetInt(SerializableFields.Coins, 0);
        int towerHealth = PlayerPrefs.GetInt(SerializableFields.TowerHealth, 0);
        int numberOfItems = PlayerPrefs.GetInt(SerializableFields.NumberOfItems, 0);
        int moneyMultiplyer = PlayerPrefs.GetInt(SerializableFields.MoneyMultiplyer, 0);


        levelLabel.text = $"Level {level}";
        coinsLabel.text = $"{coins}";

        towerHealthLabelLevel.text = $"{towerHealth + 1}";
        numberOfItemsLabelLevel.text = $"{numberOfItems + 1}";
        moneyMultiplyerLabelLevel.text = $"{moneyMultiplyer + 1}";

        towerHealthLabelPrice.text = $"{(towerHealth + 1) * GlobalConstants.IncreaseStep}";
        numberOfItemsLabelPrice.text = $"{(numberOfItems + 1) * GlobalConstants.IncreaseStep}";
        moneyMultiplyerLabelPrice.text = $"{(moneyMultiplyer + 1) * GlobalConstants.IncreaseStep}";

        UpdateButtonState();
    } 

    public void UpTowerHealth()
    {
        OnPressHandler(SerializableFields.TowerHealth, towerHealthLabelLevel, towerHealthLabelPrice);
        UpdateButtonState();

        CastleController opponentCastleController = GameObject.FindWithTag("OpponentCastle").GetComponent<CastleController>();
        CastleController playerCastleController = GameObject.FindWithTag("PlayerCastle").GetComponent<CastleController>();

        int towerHealth = PlayerPrefs.GetInt(SerializableFields.TowerHealth, 0);

        int additionalHealth = Random.Range(-2, 2);

        playerCastleController.SetCastleHealth(10 + towerHealth);
    }

    public void UpNumberOfItems()
    {
        OnPressHandler(SerializableFields.NumberOfItems, numberOfItemsLabelLevel, numberOfItemsLabelPrice);
        UpdateButtonState();
    }

    public void UpMoneyMultiplyer()
    {
        OnPressHandler(SerializableFields.MoneyMultiplyer, moneyMultiplyerLabelLevel, moneyMultiplyerLabelPrice);
        UpdateButtonState();
    }

    public void OnPressHandler(string serializableField, TextMeshProUGUI labelLevel, TextMeshProUGUI labelPrice)
    {
        int value = PlayerPrefs.GetInt(serializableField, 0);
        int newValue = value + 1;
        PlayerPrefs.SetInt(serializableField, newValue);

        labelLevel.text = $"{newValue + 1}";
        labelPrice.text = $"{(newValue + 1) * GlobalConstants.IncreaseStep}";

        int coins = PlayerPrefs.GetInt(SerializableFields.Coins, 0);
        int newCoins = coins - newValue * GlobalConstants.IncreaseStep;
        PlayerPrefs.SetInt(SerializableFields.Coins, newCoins);
        coinsLabel.text = $"{newCoins}";
    }

    public void UpdateButtonState()
    {
        int coins = PlayerPrefs.GetInt(SerializableFields.Coins, 0);
        int towerHealth = PlayerPrefs.GetInt(SerializableFields.TowerHealth, 0);
        int numberOfItems = PlayerPrefs.GetInt(SerializableFields.NumberOfItems, 0);
        int moneyMultiplyer = PlayerPrefs.GetInt(SerializableFields.MoneyMultiplyer, 0);

        towerHealthButton.interactable = ((towerHealth + 1) * GlobalConstants.IncreaseStep <= coins) ? true : false;
        towerHealthLabelPrice.alpha = ((towerHealth + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        towerHealthLabel.alpha = ((towerHealth + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        towerHealthLabelLevel.alpha = ((towerHealth + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        Color towerCoinColor = towerHealthCoin.color;
        towerCoinColor.a = ((towerHealth + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        towerHealthCoin.color = towerCoinColor;

        numberOfItemsButton.interactable = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? true : false;
        numberOfItemsLabelPrice.alpha = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        numberOfItemsLabel.alpha = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        numberOfItemsLabelLevel.alpha = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        Color numberOfItemsColor = numberOfItemsCoin.color;
        numberOfItemsColor.a = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        numberOfItemsCoin.color = numberOfItemsColor;

        moneyMultiplyerButton.interactable = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? true : false;
        moneyMultiplyerLabelPrice.alpha = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        moneyMultiplyerLabel.alpha = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        moneyMultiplyerLabelLevel.alpha = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        Color moneyMultiplyerColor = moneyMultiplyerCoin.color;
        moneyMultiplyerColor.a = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? 255f : 0.2f;
        moneyMultiplyerCoin.color = moneyMultiplyerColor;
    }

    public void OnStartGame()
    {
        GameManager.Instance.SetGameState(GameState.Game);
    }
}
