using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsManager : MonoBehaviour
{
    public const string PREF_LAST_INIT_SETTINGS_VERSION = "PREF_LAST_INITIALIZED_SETTINGS_VERSION"; 
    public const string PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE = "PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE";
    void Awake()
    {
        //initialize settings from version
        Debug.Log("Settings version initialized on awake: " + PlayerPrefsPro.GetString(PREF_LAST_INIT_SETTINGS_VERSION));
        if (PlayerPrefsPro.GetString(PREF_LAST_INIT_SETTINGS_VERSION) != Application.version)
        {
            InitializePrefSettings();
        }

        
    }
    void InitializePrefSettings()
    {
        Debug.Log("Overwriting last initialized prefs from settings: " + Application.version);
        PlayerPrefsPro.SetBool(PREF_SETTINGS_ALLOW_BONUS_RV_BUBBLE, true);

        //reflect most recent settings version
        PlayerPrefsPro.SetString(PREF_LAST_INIT_SETTINGS_VERSION, Application.version);    
        Debug.Log("NEW V SET: " + PlayerPrefsPro.GetString(PREF_LAST_INIT_SETTINGS_VERSION));
        PlayerPrefsPro.Save();

    }
}
