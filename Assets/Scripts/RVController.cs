using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using GameAnalyticsSDK;

public enum AdOfferSource
{
    purseButton = 0,
    movingOfferBubble = 1,
    fillDefecitForRecycle = 2,
    fillDefecitForShuffle = 3
}

public class RVController : MonoBehaviour, IUnityAdsListener { 

    public PowerupsPanelController powerupsPanel;
    public WatchRVPanelController watchRVPanelController;

    public MovingRVButton rvButtonPrefab;
    public Transform rvBubbleParent;

    private CurrencyAmount rewardPowerupType = CurrencyAmount.CoinsSmall;

    private float lifetimeGameSeconds;
    private float timeLastSurfacedAdOffer;
    private MovingRVButton currentOffer;
    private AdOfferSource currentSource;

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
                if (PlayerPrefsPro.GetBool("PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE"))
                {
                    timeLastSurfacedAdOffer = Time.time;
                    SurfaceRVOption(CurrencyAmount.CoinsMedium);
                }
            }
        }
    }

    public void Show(string placementId, AdOfferSource source)
    {
        currentSource = source;
        Advertisement.Show(placementId);
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, AnalyticsKeys.unitySDK, nameof(currentSource));
    }

    public bool IsAdReady()
    {
        return Advertisement.IsReady();
    }

    public void SurfaceRVOption(CurrencyAmount powerupType)
    {
        if (Advertisement.IsReady() && GameController.IsTutorialComplete() && GameController.GetGameState() == GameState.InTrial)
        {
            SetPowerupType(powerupType);
            currentOffer = Instantiate<MovingRVButton>(rvButtonPrefab, rvBubbleParent);
            currentOffer.Initialize(rewardPowerupType, RequestAd, HandleRVRequestDestroyed);
        }
    }

    public void SetPowerupType(CurrencyAmount powerupTypeSet)
    {
        rewardPowerupType = powerupTypeSet;
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
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, AnalyticsKeys.unitySDK, nameof(currentSource));
            RewardForAdSuccess();
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
            GameAnalytics.NewAdEvent(GAAdAction.Undefined, GAAdType.RewardedVideo, AnalyticsKeys.unitySDK, nameof(currentSource) + "_SKIPPED");
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
            GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, AnalyticsKeys.unitySDK, nameof(currentSource));

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



    public void RequestAd(CurrencyAmount type, AdOfferSource adSource)
    {
        if (IsAdReady())
        {
            Time.timeScale = 0f;
            watchRVPanelController.Show(type, adSource);
        }
        else
        {
            AnalyticsController.OnAdUnavailable();
        }
    }

    private void RewardForAdSuccess()
    {
        Vector3 centerScreen = Camera.main.ViewportToScreenPoint(Vector3.one * .5f);
        MoneyController.AwardCoins(new Vector2(centerScreen.x, centerScreen.y), WatchRVPanelController.GetCoinAmountForPackageSize(rewardPowerupType), Random.Range(15,25));
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