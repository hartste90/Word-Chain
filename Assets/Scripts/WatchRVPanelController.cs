using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
public class WatchRVPanelController : MonoBehaviour
{
    public TextMeshProUGUI panelText;

    public void Show()
    {
        panelText.text = "Watch ad to claim <sprite=0>x100?";
        gameObject.SetActive(true);
    }

    public void ShowNeedCoins(int coinsNeeded)
    {
        panelText.text = "Need more coins\nWatch ad to claim <sprite=0>x300?";
        gameObject.SetActive(true);
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
        Advertisement.Show(myPlacementId);

        Hide();   
    }

    
}
