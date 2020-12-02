using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyCoinsButtonController : MonoBehaviour
{
    UnityAction<PowerupType> pressedCallback;
    public void SetPressedCallback(UnityAction<PowerupType> callbackSet)
    {
        pressedCallback = callbackSet;
    }

    public void OnPressed()
    {
        pressedCallback?.Invoke(PowerupType.CoinsSmall);
    }
}
