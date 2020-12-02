using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
public class WatchRVPanelController : MonoBehaviour
{
    public TextMeshProUGUI panelText;

    private PowerupType latestCoinPackageType;

    public void Show(PowerupType type)
    {
        panelText.text = "Watch ad to claim <sprite=0>x" + GetCoinAmountForPackageSize(type) + "?";
        latestCoinPackageType = type;
        gameObject.SetActive(true);
    }

    public void ShowNeedCoins(int coinsNeeded)
    {
        PowerupType packageSize = GetPackageTypeFromCoinsNeed(coinsNeeded);
        panelText.text = "Not enough coins\nWatch ad to claim <sprite=0>x" + GetCoinAmountForPackageSize(packageSize) + "?";
        latestCoinPackageType = packageSize;
        gameObject.SetActive(true);
    }

    public static PowerupType GetPackageTypeFromCoinsNeed(int coinsNeeded)
    {
        if (coinsNeeded <= 100)
            return PowerupType.CoinsSmall;
        else if (coinsNeeded <= 300)
            return PowerupType.CoinsMedium;
        else if (coinsNeeded <= 750)
            return PowerupType.CoinsLarge;
        else if (coinsNeeded <= 1350)
            return PowerupType.CoinsHuge;
        else
            return PowerupType.CoinsSmall;
    }

    public static int GetCoinAmountForPackageSize(PowerupType type)
    {
        switch(type)
        {
            case PowerupType.CoinsSmall:
                return 100;
            case PowerupType.CoinsMedium:
                return 300;
            case PowerupType.CoinsLarge:
                return 750;
            case PowerupType.CoinsHuge:
                return 1350;
            default:
                return 0;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnNoButtonPressed()
    {
        Hide();
        Time.timeScale = 1f;
    }

    public void OnYesButtonPressed()
    {
        //play ad
        string myPlacementId = "rewardedVideo";
        GameController.Instance.rVController.SetPowerupType(latestCoinPackageType);
        Advertisement.Show(myPlacementId);

        Hide();   
    }

    
}
