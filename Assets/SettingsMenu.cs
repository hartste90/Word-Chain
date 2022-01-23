using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Toggle _allowBubbleRVToggle;    

    public void Open()
    {
        //initialize settings from prefs
        _allowBubbleRVToggle.isOn = (PlayerPrefsPro.GetBool(PrefsManager.PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE));

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ToggleAllowBubbleRV(bool isOnSet)
    {
        Debug.Log("Setting ALLOW RV: " + isOnSet);
        PlayerPrefsPro.SetBool(PrefsManager.PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE , isOnSet);
        PlayerPrefsPro.Save();
    }
}
