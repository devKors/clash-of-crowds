using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryMenu : MonoBehaviour
{
    public TextMeshProUGUI coinsLabel;


    void OnEnable()
    {
        int moneyMultiplyer = PlayerPrefs.GetInt(SerializableFields.MoneyMultiplyer, 0);
        int earned = GlobalConstants.PermanentVictoryReward + (GlobalConstants.AdditionalVictoryReward * moneyMultiplyer);
        coinsLabel.text = $"{earned}";
    }
}
