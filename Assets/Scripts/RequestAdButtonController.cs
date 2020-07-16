using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
public class RequestAdButtonController : MonoBehaviour
{
    public RVController rVController;
    public Button button;

    void Update()
    {
        button.interactable = Advertisement.IsReady();
    }

    public void OnRequestAdButtonPressed()
    {
        rVController.RequestAd();
    }

}
