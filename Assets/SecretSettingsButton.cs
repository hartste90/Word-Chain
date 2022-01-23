using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSettingsButton : MonoBehaviour
{
    [SerializeField]
    private int _secretTapThreshold = 10;
    [SerializeField]
    private int _secretTimeThreshold = 5;
    [SerializeField]
    private SettingsMenu _settingsMenu;
    
    private int _activePressCount = 0;
    private float _lastTimeReset = 0f;
    // Update is called once per frame
    void Update()
    {
        if (_activePressCount > 0 
            && Time.time - _lastTimeReset > _secretTimeThreshold
            )
        {
            _lastTimeReset = Time.time;
            _activePressCount = 0;
            Debug.Log("Resetting secret menu count");
        }
    }

    public void OnSecretButtonPressed()
    {
        Debug.Log("Secret menu tap count: " + _activePressCount);
        if (_activePressCount == 0)
        {
            _lastTimeReset = Time.time;
        }
        _activePressCount++;
        if (_activePressCount == _secretTapThreshold)
        {
            OpenSettingsMenu();
            _activePressCount = 0;
        }
    }

    void OpenSettingsMenu()
    {
        _settingsMenu.Open();
    }
}
