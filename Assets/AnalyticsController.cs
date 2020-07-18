using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
public class AnalyticsController : MonoBehaviour
{
    void Start()
    {
        GameAnalytics.Initialize();
    }

}
