using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class WatchRVPanelController : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnNoButtonPressed()
    {
        Hide();
    }

    public void OnYesButtonPressed()
    {
        //play ad
        string myPlacementId = "rewardedVideo";
        Advertisement.Show(myPlacementId);

        Hide();   
    }

    
}
