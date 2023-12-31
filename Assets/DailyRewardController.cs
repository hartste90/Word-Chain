﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class DailyRewardController : MonoBehaviour
{
    const int DAYS_BETWEEN_REWARD = 1;
    const int DAYS_TO_COLLECT_REWARD_BEFORE_RESET = 2;
    private int consecutiveDaysClaimed;

    public RewardPanelController dailyRewardPanel;
    public List<DailyRewardData> rewardsListByDay;
    private UnityAction onClaimButtonPressed;

    public void Init()
    {
        dailyRewardPanel.Init(OnClaimButtonPressed);

        if (LastClaimWasPassedDue())
        {
            consecutiveDaysClaimed = 0;
        }
        else
        {
            consecutiveDaysClaimed = PlayerPrefs.GetInt(AnalyticsKeys.consecutive_days_claimed, 0);
        }
        CheckShowReward();
        
    }

    private bool LastClaimWasPassedDue()
    {
        //returns true if last claim time was greater than 1 day ago
        DateTime lastRewardTime;
        bool hasEverHadReward = DateTime.TryParse(PlayerPrefs.GetString(AnalyticsKeys.last_daily_reward_time, DateTime.Now.AddDays(-100).ToString()), out lastRewardTime);
        if (hasEverHadReward)
        {
            int lastClaimTimeCompare = lastRewardTime.AddDays(DAYS_TO_COLLECT_REWARD_BEFORE_RESET).CompareTo(DateTime.Now);
            
            if(lastClaimTimeCompare <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckShowReward()
    {
        if (IsRewardReady() && GameController.GetGameState() == GameState.InTrial)
        {
            Show();
        }
    }

    public bool IsRewardReady()
    {
        DateTime now = DateTime.Now;
        DateTime lastRewardTime = DateTime.Now.AddDays(-100);
        bool hasEverHadReward = DateTime.TryParse(PlayerPrefs.GetString(AnalyticsKeys.last_daily_reward_time, DateTime.Now.AddDays(-100).ToString()), out lastRewardTime);
        if (hasEverHadReward)
        {
            int lastRewardTimeCompare = lastRewardTime.AddDays(DAYS_BETWEEN_REWARD).CompareTo(now);
            if (lastRewardTimeCompare <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public void Show()
    {
        dailyRewardPanel.Show(IsRewardReady(), consecutiveDaysClaimed, GetCurrentAwardAmount());
    }

    public void OnClaimButtonPressed()
    {
        onClaimButtonPressed?.Invoke();
        consecutiveDaysClaimed++;
        PlayerPrefs.SetInt(AnalyticsKeys.consecutive_days_claimed, consecutiveDaysClaimed);
        ClaimReward();
        SoundController.PlayUITap();
    }

    public int GetCurrentAwardAmount()
    {
        return rewardsListByDay[Mathf.Min(consecutiveDaysClaimed, rewardsListByDay.Count-1)].rewardAmt;
    }

    private void ClaimReward()
    {
        int amt = GetCurrentAwardAmount();
        MoneyController.AwardCoins(dailyRewardPanel.GetDooberSource(), amt, amt);
        PlayerPrefs.SetString(AnalyticsKeys.last_daily_reward_time, DateTime.Now.ToString());
        AnalyticsController.OnClaimDailyReward(amt, consecutiveDaysClaimed);
    }


}


[Serializable]
public class DailyRewardData
{
    public int rewardAmt;
}