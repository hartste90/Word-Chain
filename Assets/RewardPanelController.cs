using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class RewardPanelController : MonoBehaviour
{
    public Transform rewardReadyPanel;
    public Transform rewardNotReadyPanel;
    public Transform dooberSource;
    public TextMeshProUGUI consecutiveDaysLabel;
    public TextMeshProUGUI currentRewardAmountLabel;
    public Transform giftImage;
    public Transform button;


    private UnityAction onClaimButtonPressed;
    private CanvasGroup cg;

    public void Init(UnityAction claimCallback)
    {
        onClaimButtonPressed += claimCallback;
        cg = GetComponent<CanvasGroup>();
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

            AnimateIntro();
        }
    }

    private void AnimateIntro()
    {
        gameObject.SetActive(true);
        transform.localRotation = Quaternion.Euler(Vector3.forward * 45f);
        transform.localScale = Vector3.one * 1.5f;
        cg.alpha = 0f;

        transform.DOLocalRotate(Vector3.zero, .25f);
        transform.DOScale(Vector3.one, .25f);
        cg.DOFade(1f, .1f);

        DOTween.Kill("daily_reward_gift_seq");
        Sequence giftSeq = DOTween.Sequence();
        giftSeq.Append(giftImage.DOLocalJump(giftImage.localPosition, 50, 1, 1f).SetLoops(-1).SetEase(Ease.OutBounce));
        giftSeq.AppendInterval(3f);
        giftSeq.SetLoops(-1);
        giftSeq.SetId("daily_reward_gift_seq");
        giftSeq.Play();

        DOTween.Kill("daily_reward_button_seq");
        Sequence buttonSeq = DOTween.Sequence();
        buttonSeq.AppendInterval(2f);
        buttonSeq.Append(button.DOShakeRotation(.5f, Vector3.forward * 20f).SetLoops(-1).SetEase(Ease.Linear));
        buttonSeq.SetLoops(-1);
        buttonSeq.SetId("daily_reward_button_seq");
        buttonSeq.Play();
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
