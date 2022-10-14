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

    public Button numberOfItemsButton;
    public TextMeshProUGUI numberOfItemsLabelLevel;
    public TextMeshProUGUI numberOfItemsLabelPrice;

    public Button moneyMultiplyerButton;
    public TextMeshProUGUI moneyMultiplyerLabelLevel;
    public TextMeshProUGUI moneyMultiplyerLabelPrice;
    

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

        towerHealthLabelLevel.text = $"Tower {towerHealth + 1}";
        numberOfItemsLabelLevel.text = $"Items {numberOfItems + 1}";
        moneyMultiplyerLabelLevel.text = $"Money {moneyMultiplyer + 1}";

        towerHealthLabelPrice.text = $"Price {(towerHealth + 1) * GlobalConstants.IncreaseStep}";
        numberOfItemsLabelPrice.text = $"Price {(numberOfItems + 1) * GlobalConstants.IncreaseStep}";
        moneyMultiplyerLabelPrice.text = $"Price {(moneyMultiplyer + 1) * GlobalConstants.IncreaseStep}";

        UpdateButtonState();
    } 

    public void UpTowerHealth()
    {
        OnPressHandler(SerializableFields.TowerHealth, towerHealthLabelLevel, towerHealthLabelPrice);
        UpdateButtonState();
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

        labelLevel.text = $"Level {newValue + 1}";
        labelPrice.text = $"Price {(newValue + 1) * GlobalConstants.IncreaseStep}";

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
        numberOfItemsButton.interactable = ((numberOfItems + 1) * GlobalConstants.IncreaseStep <= coins) ? true : false;
        moneyMultiplyerButton.interactable = ((moneyMultiplyer + 1) * GlobalConstants.IncreaseStep <= coins) ? true : false;
    }
}
