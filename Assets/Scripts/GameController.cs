using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    #region singleton
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameController>();
                if (instance == null)
                {
                    Debug.LogError("No GameController was found in this scene. Make sure you place one in the scene.");
                    return instance;
                }
            }
            return instance;
        }
    }
    #endregion


    /* 
    1. spawn tiles
    2. record user input and show current word
    3. allow submission
    4. valid word -> clear word, replace used letters
    5. allow backspace
    */

    public TrialController trialPrefab;

    public Transform trialParent;
    public BackgroundController backgroundController;
    public RVController rVController;
    private TrialController trialController;


    

    void Start()
    {
        InitializeSDKs();
        InitializeDictionary();
        BeginTrial();

    }

    private void InitializeSDKs()
    {
        //ads
        InitializeAdsSDK();
        
    }

    private void InitializeAdsSDK()
    {
#if UNITY_IOS
        string gameId = "3699028";
#elif UNITY_ANDROID
        string gameId = "3699029";
#endif

        bool testMode = true;
        Advertisement.Initialize (gameId, testMode);
    }

    private void InitializeDictionary()
    {
        DictionaryController.ReadExternalDictionary();
    }
    

    private void BeginTrial()
    {
        trialController = Instantiate<TrialController>(trialPrefab, trialParent);
        trialController.SetGameController(this);
        rVController.SetPowerupsPanel(trialController.GetPowerupsPanel());
        trialController.BeginTrial();
    }
        

    

    public void OnBackspaceButtonPressed()
    {
        trialController.Backspace();
    }

    

    public void OnClearAllButtonPressed()
    {
        trialController.ClearAllTiles();
        
    }

    public void OnSubmitButtonPressed(string word)
    {
        CreateInBackground(word);
    }

    

    private void CreateInBackground(string word)
    {
        backgroundController.SpawnWord(word);
    }

    public void OnShuffleButtonPressed()
    {
        trialController.ShuffleTiles();
    }


    public void RecycleBoard()
    {
        trialController.RecycleBoard();
    }
    

}
