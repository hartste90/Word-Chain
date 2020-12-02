using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
using DG.Tweening;
using GameAnalyticsSDK;

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
    public TrialEndPanel trialEndPanel;
    public RVController rVController;
    private TrialController trialController;

    public GameObject inputBlocker;


    

    void Start()
    {
        InitializeSDKs();
        InitializeDictionary();
        InitializeControllers();
        SetControllerCallbacks();
        BeginTrial();

    }

    private void InitializeSDKs()
    {
        //ads
        InitializeAdsSDK();
        
    }

    private void InitializeControllers()
    {
        //MoneyController.Init()
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

    private void SetControllerCallbacks()
    {
        trialEndPanel.SetTrialEndCallback(BeginTrial);
    }
    

    private void BeginTrial()
    {
        DisableInput();
        trialController = Instantiate<TrialController>(trialPrefab, trialParent);
        trialController.SetGameController(this);
        rVController.SetPowerupsPanel(trialController.GetPowerupsPanel());
        trialController.BeginTrial();
    }

    public void EndTrial()
    {
        Destroy(trialController.gameObject);
        rVController.DestroyCurrentOffer();
        backgroundController.DestroyAllBlocks();
        trialEndPanel.Show();
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
        DisableInput();
    }

    

    private void CreateInBackground(string word)
    {
        backgroundController.SpawnWord(word);
    }

    //public void OnShuffleButtonPressed()
    //{
    //    trialController.ShuffleTiles();
    //}


    //public void RecycleBoard()
    //{
    //    trialController.RecycleBoard();
    //}

    public void DisableInput()
    {
        inputBlocker.SetActive(true);
    }

    public void EnableInput()
    {
        inputBlocker.SetActive(false);
    }
    

}
