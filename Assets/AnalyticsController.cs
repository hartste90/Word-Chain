using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class AnalyticsKeys
{
    public static string userLevelsBegun = "user_levels_begun";
    public static string userLevelsCompleted = "user_levels_complete";
    public static string unitySDK = "unitySDK";
    public static string userBeginTrial = "user_begin_trial";
    public static string userCompleteTrial = "user_complete_trial";

    public static string ad_unavailable = "ad_unavailable";

    public static string reject_coin_offer = "rejected_coin_offer";
    public static string accept_coin_offer = "accepted_coin_offer";

    public static string words_this_session = "words_this_session";
    public static string word_for_quest = "word_for_quest";


    public static string failed_use_powerup = "failed_use_powerup";
    public static string use_powerup = "use_powerup";

    public static string used_coin_letter = "used_coin_letter";
    public static string word_length = "word_length";

    //tutorial
    public static string is_tutorial_complete = "is_tutorial_complete";

    //daily rewards
    public static string last_daily_reward_time = "last_daily_reward_time";
    public static string consecutive_days_claimed = "consecutive_days_played";
    public static string daily_reward_days = "daily_reward_days";

    //settings
    public static string is_muted = "is_muted";
}


public class AnalyticsController : MonoBehaviour
{
    void Awake()
    {
        GameAnalytics.Initialize();
    }

    public static void OnBeginTrial()
    {
        int levelsBegun = AnalyticsController.GetLevelsBegun();
        PlayerPrefs.SetInt(AnalyticsKeys.userLevelsBegun, levelsBegun + 1);
        GameAnalytics.NewDesignEvent(AnalyticsKeys.userBeginTrial, levelsBegun);
    }

    public static void OnCompleteTrial()
    {
        int levelsCompleted = PlayerPrefs.GetInt(AnalyticsKeys.userLevelsCompleted, 0);
        int wordsCompletedThisSession = PlayerPrefs.GetInt(AnalyticsKeys.words_this_session, 0);
        PlayerPrefs.SetInt(AnalyticsKeys.userLevelsCompleted, levelsCompleted + 1);
        GameAnalytics.NewDesignEvent(AnalyticsKeys.userCompleteTrial, levelsCompleted);
        GameAnalytics.NewDesignEvent(AnalyticsKeys.words_this_session, wordsCompletedThisSession);
    }

    public static void OnFailedPowerup(PowerupType powerupType)
    {
        GameAnalytics.NewDesignEvent(AnalyticsKeys.failed_use_powerup + ":" + nameof(powerupType), MoneyController.GetCurrentMoney());
    }

    public static void OnUsePowerup(PowerupType powerupType)
    {

        GameAnalytics.NewDesignEvent(AnalyticsKeys.use_powerup + ":" + nameof(powerupType), MoneyController.GetCurrentMoney());
    }

    public static void OnAcceptCoinOffer()
    {
        GameAnalytics.NewDesignEvent(AnalyticsKeys.accept_coin_offer+"AtCost", MoneyController.GetCurrentMoney());
        GameAnalytics.NewDesignEvent(AnalyticsKeys.accept_coin_offer + "AtLevel", GetLevelsBegun());
    }

    public static void OnRejectCoinOffer()
    {
        GameAnalytics.NewDesignEvent(AnalyticsKeys.reject_coin_offer + "AtCost", MoneyController.GetCurrentMoney());
        GameAnalytics.NewDesignEvent(AnalyticsKeys.reject_coin_offer + "AtLevel", GetLevelsBegun());
    }

    public static void OnUseCoinLetter()
    {
        GameAnalytics.NewDesignEvent(AnalyticsKeys.used_coin_letter);
    }

    public static void OnAdUnavailable()
    {
        GameAnalytics.NewDesignEvent(AnalyticsKeys.ad_unavailable);
    }

    public static void OnWordSubmitted(string word, bool isWordForQuest)
    {
        int wordLength = word.Length;
        GameAnalytics.NewDesignEvent(AnalyticsKeys.word_length, wordLength);
        GameAnalytics.NewDesignEvent(AnalyticsKeys.word_for_quest, isWordForQuest ? 1 : 0);
    }

    public static void OnTutorialStarted()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, AnalyticsKeys.is_tutorial_complete);
    }

    public static void OnTutorialCompleted()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, AnalyticsKeys.is_tutorial_complete);
    }

    public static void OnClaimDailyReward(int amt, int consecutiveDaysClaimed)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "coins", consecutiveDaysClaimed, AnalyticsKeys.daily_reward_days, "");
    }

    private static int GetLevelsBegun()
    {
        return PlayerPrefs.GetInt(AnalyticsKeys.userLevelsBegun, 0);
    }

}
