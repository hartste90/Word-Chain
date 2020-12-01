using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PurseController : MonoBehaviour
{
    public BuyCoinsButtonController buyCoinsButton;
    public TextMeshProUGUI coinsLabel;

    public void Init(UnityAction addCoinsButtonPressedCallbackSet)
    {
        buyCoinsButton.SetPressedCallback(addCoinsButtonPressedCallbackSet);
        MoneyController.onMoneyChanged += UpdateCoins;
        UpdateCoins(MoneyController.GetCurrentMoney());
    }

    public void UpdateCoins(int currentCoinsCount)
    {
        coinsLabel.text = currentCoinsCount.ToString();
    }

    private void OnDestroy()
    {
        MoneyController.onMoneyChanged -= UpdateCoins;
    }
}
