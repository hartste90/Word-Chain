using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
public class RVController : MonoBehaviour, IUnityAdsListener { 

    public PowerupsPanelController powerupsPanel;
    public WatchRVPanelController watchRVPanelController;

    public MovingRVButton rvButtonPrefab;
    public Transform rvBubbleParent;

    private PowerupType rewardPowerupType = PowerupType.CoinsSmall;

    private float lifetimeGameSeconds;
    private float timeLastSurfacedAdOffer;
    private MovingRVButton currentOffer;

    // Initialize the Ads listener and service:
    void Start () {
        Advertisement.AddListener (this);
        lifetimeGameSeconds = PlayerPrefsPro.GetFloat("LIFETIME_GAME_SECONDS", 0f);
    }

    void Update()
    {
        if (Time.time - lifetimeGameSeconds > 5f)
        {
            lifetimeGameSeconds = Time.time;
            PlayerPrefsPro.SetFloat("LIFETIME_GAME_SECONDS", lifetimeGameSeconds);
        }

        if (lifetimeGameSeconds > 60f)
        {
            if (Time.time - timeLastSurfacedAdOffer > 60f)
            {
                timeLastSurfacedAdOffer = Time.time;
                SurfaceRVOption(PowerupType.CoinsSmall);
            }
        }
    }

    public void SurfaceRVOption(PowerupType powerupType)
    {
        if (Advertisement.IsReady())
        {
            rewardPowerupType = powerupType;
            currentOffer = Instantiate<MovingRVButton>(rvButtonPrefab, rvBubbleParent);
            currentOffer.Initialize(rewardPowerupType, RequestAd, HandleRVRequestDestroyed);
        }
    }

    public void DestroyCurrentOffer()
    {
        if (currentOffer != null)
        {
            currentOffer.PlayBubbleOutAnimation();
        }
    }

    public void HandleRVRequestDestroyed()
    {
        currentOffer = null;
        timeLastSurfacedAdOffer = Time.time;
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
        else if(rewardPowerupType == PowerupType.CoinsSmall)
        {
            MoneyController.ChangeMoney(100);
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