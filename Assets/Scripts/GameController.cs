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
    public static int LONG_WORD_CHARACTER_REQUIREMENT = 6;

    public Transform trialParent;
    public BackgroundController backgroundController;
    public TrialEndPanel trialEndPanel;
    public RVController rVController;
    private TrialController trialController;
    public TutorialController tutorialController;
    public DailyRewardController dailyRewardController;
    public GameObject inputBlocker;

    public MuteButton muteButton;

    private GameState gamestate = GameState.Undefined;


    void Start()
    {
        InitializeSDKs();
        InitializeDictionary();
        InitializeControllers();
        SetControllerCallbacks();
        InitializeDailyReward();
        InitializeTutorial();
        BeginTrial();
        

    }

    private void OnApplicationFocus(bool focus)
    {
        dailyRewardController.CheckShowReward();
    }

    private void InitializeDailyReward()
    {
        dailyRewardController.Init();
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

        bool testMode = Debug.isDebugBuild;
        // Advertisement.Initialize (gameId, testMode);
    }

    private void InitializeDictionary()
    {
        DictionaryController.ReadExternalDictionary();
    }

    private void InitializeTutorial()
    {
        tutorialController.Init();
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
        gamestate = GameState.InTrial;
        dailyRewardController.CheckShowReward();
        
    }

    public void EndTrial()
    {
        gamestate = GameState.Undefined;
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

    public static Vector2 GetPurseScreenPosition()
    {
        return Instance.trialController.purseController.GetDooberTarget();
    }

    public static void UpdateMuteUI(bool isMuted)
    {
        Instance.muteButton.UpdateMuteUI(isMuted);
    }

    public static bool IsTutorialComplete()
    {
        return Instance.tutorialController.IsTutorialComplete();
    }

    public static GameState GetGameState()
    {
        return Instance.gamestate;
    }

}

public enum PowerupType
{
    recycle = 0,
    shuffle = 1,
    wildcard = 2
}

public enum GameState
{
    Undefined = 0,
    InTrial = 1
}