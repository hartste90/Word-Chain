using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyCoinsButtonController : MonoBehaviour
{
    UnityAction pressedCallback;
    public void SetPressedCallback(UnityAction callbackSet)
    {
        pressedCallback = callbackSet;
    }

    public void OnPressed()
    {
        pressedCallback?.Invoke();
    }
}
