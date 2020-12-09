using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class RewardPanelController : MonoBehaviour
{
    public Transform rewardReadyPanel;
    public Transform rewardNotReadyPanel;
    public Transform dooberSource;
    public TextMeshProUGUI consecutiveDaysLabel;
    public TextMeshProUGUI currentRewardAmountLabel;
    private UnityAction onClaimButtonPressed;

    public void Init(UnityAction claimCallback)
    {
        onClaimButtonPressed += claimCallback;
    }

    public void Show(bool isRewardReady, int consecutiveDays, int rewardAmount)
    {
        if (PlayerPrefs.GetInt(AnalyticsKeys.is_tutorial_complete, 0) == 1)
        {
            rewardReadyPanel.gameObject.SetActive(isRewardReady);
            rewardNotReadyPanel.gameObject.SetActive(!isRewardReady);
            if (isRewardReady)
            {
                PopulateRewardPanel(consecutiveDays, rewardAmount);
            }

            gameObject.SetActive(true);
        }
    }

    private void PopulateRewardPanel(int consecutiveDays, int amountSet)
    {
        consecutiveDaysLabel.text = "Day " + consecutiveDays;
        currentRewardAmountLabel.text = amountSet + "x <sprite=0>";

    }

    public void OnClaimButtonPressed()
    {
        onClaimButtonPressed?.Invoke();
        ClosePanel();
    }

    public void OnCloseButtonPressed()
    {
        ClosePanel();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public Vector2 GetDooberSource()
    {
        return dooberSource.position;
    }
}
