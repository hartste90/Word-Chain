using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyCoinsButtonController : MonoBehaviour
{
    UnityAction<CurrencyAmount, AdOfferSource> pressedCallback;
    public void SetPressedCallback(UnityAction<CurrencyAmount, AdOfferSource> callbackSet)
    {
        pressedCallback = callbackSet;
    }

    public void OnPressed()
    {
        pressedCallback?.Invoke(CurrencyAmount.CoinsSmall, AdOfferSource.purseButton);
    }
}
