using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;

public class PurseController : MonoBehaviour
{
    public BuyCoinsButtonController buyCoinsButton;
    public TextMeshProUGUI coinsLabel;
    public Transform dooberTarget;
    public CanvasGroup removedCoinsPanel;
    public TextMeshProUGUI removedCoinsLabel;

    private Vector3 ogScale;
    private string ctaBumpId = "PurseBumpCTA";

    public void Init(UnityAction<CurrencyAmount, AdOfferSource> addCoinsButtonPressedCallbackSet)
    {
        buyCoinsButton.SetPressedCallback(addCoinsButtonPressedCallbackSet);
        ogScale = transform.localScale;
        MoneyController.onMoneyChanged += UpdateCoins;
        MoneyController.onCoinAnimationComplete += PlayCTABump;
        UpdateCoins(MoneyController.GetCurrentMoney());
    }

    public void UpdateCoins(int currentCoinsCount)
    {
        coinsLabel.text = currentCoinsCount.ToString();
    }

    private void OnDestroy()
    {
        MoneyController.onMoneyChanged -= UpdateCoins;
    }

    public void PlayCTABump()
    {
        DOTween.Kill(ctaBumpId);
        transform.localScale = ogScale;
        transform.DOPunchScale(Vector3.one * .2f, .2f).SetId(ctaBumpId);
    }

    public Vector2 GetDooberTarget()
    {
        return dooberTarget.position;
    }
}
