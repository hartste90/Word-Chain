using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
public class RVController : MonoBehaviour, IUnityAdsListener { 

    public PowerupsPanelController powerupsPanel;
    public WatchRVPanelController watchRVPanelController;

    private PowerupType rewardPowerupType;

    // Initialize the Ads listener and service:
    void Start () {
        Advertisement.AddListener (this);
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
    }

    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, show the ad:
        // if (placementId == myPlacementId) {
        //     Advertisement.Show (myPlacementId);
        // }
    }

    public void RequestAd(PowerupType powerupType)
    {
        rewardPowerupType = powerupType;
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
}