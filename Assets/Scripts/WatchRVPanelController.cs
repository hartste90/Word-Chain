using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
public class WatchRVPanelController : MonoBehaviour
{
    public TextMeshProUGUI panelText;

    private CurrencyAmount latestCoinPackageType;
    private AdOfferSource source;

    public void Show(CurrencyAmount type, AdOfferSource sourceSet)
    {
        source = sourceSet;
        panelText.text = "Watch ad to claim <sprite=0>x" + GetCoinAmountForPackageSize(type) + "?";
        latestCoinPackageType = type;
        gameObject.SetActive(true);
    }

    public void ShowNeedCoins(int coinsNeeded, AdOfferSource sourceSet)
    {
        source = sourceSet;
        CurrencyAmount packageSize = GetPackageTypeFromCoinsNeed(coinsNeeded);
        panelText.text = "Not enough coins\nWatch ad to claim <sprite=0>x" + GetCoinAmountForPackageSize(packageSize) + "?";
        latestCoinPackageType = packageSize;
        gameObject.SetActive(true);
    }

    public static CurrencyAmount GetPackageTypeFromCoinsNeed(int coinsNeeded)
    {
        if (coinsNeeded <= 100)
            return CurrencyAmount.CoinsSmall;
        else if (coinsNeeded <= 300)
            return CurrencyAmount.CoinsMedium;
        else if (coinsNeeded <= 750)
            return CurrencyAmount.CoinsLarge;
        else if (coinsNeeded <= 1350)
            return CurrencyAmount.CoinsHuge;
        else
            return CurrencyAmount.CoinsSmall;
    }

    public static int GetCoinAmountForPackageSize(CurrencyAmount type)
    {
        switch(type)
        {
            case CurrencyAmount.CoinsSmall:
                return 100;
            case CurrencyAmount.CoinsMedium:
                return 300;
            case CurrencyAmount.CoinsLarge:
                return 750;
            case CurrencyAmount.CoinsHuge:
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
        AnalyticsController.OnRejectCoinOffer();
        SoundController.PlayUITap();
    }

    public void OnYesButtonPressed()
    {
        //play ad
        string myPlacementId = "rewardedVideo";
        GameController.Instance.rVController.SetPowerupType(latestCoinPackageType);
        GameController.Instance.rVController.Show(myPlacementId, source);
        SoundController.PlayUITap();

        Hide();   
    }

    
}
