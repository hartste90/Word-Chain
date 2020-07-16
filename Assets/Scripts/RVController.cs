﻿using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
public class RVController : MonoBehaviour, IUnityAdsListener { 

    public PowerupsPanelController powerupsPanel;
    public WatchRVPanelController watchRVPanelController;

    public MovingRVButton rvButtonPrefab;
    public Transform rvBubbleParent;

    private PowerupType rewardPowerupType = PowerupType.NewBoard;

    // Initialize the Ads listener and service:
    void Start () {
        Advertisement.AddListener (this);
        if (Advertisement.IsReady())
        {
            SurfaceRVOption(PowerupType.NewBoard);
        }
    }

    public void SurfaceRVOption(PowerupType powerupType)
    {
        rewardPowerupType = powerupType;
        MovingRVButton rvButton = Instantiate<MovingRVButton>(rvButtonPrefab, rvBubbleParent);
        rvButton.Initialize(rewardPowerupType, RequestAd);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            RewardForAdSuccess();
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
        Time.timeScale = 1f;
    }

    public void OnUnityAdsReady (string placementId) {
        // SurfaceRVOption(PowerupType.NewBoard);
        // If the ready Placement is rewarded, show the ad:
        // if (placementId == myPlacementId) {
        //     Advertisement.Show (myPlacementId);
        // }
    }



    public void RequestAd()
    {
        Time.timeScale = 0f;
        watchRVPanelController.Show();
    }

    private void RewardForAdSuccess()
    {
        if (rewardPowerupType == PowerupType.NewBoard)
        {
            powerupsPanel.AddNewBoardPowerup();
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }

    public void SetPowerupsPanel(PowerupsPanelController powerupsPanelSet)
    {
        powerupsPanel = powerupsPanelSet;
    }
}