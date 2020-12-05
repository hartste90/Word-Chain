using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using GameAnalyticsSDK;

public class TrialController : MonoBehaviour
{

    const int SHUFFLE_LETTERS_COST = 20;
    const int RECYCLE_LETTERS_COST = 300;

    public TextMeshProUGUI currentWordText;

    public PurseController purseController;

    public LetterTileController tilePrefab;
    public Transform tileGroupParent;
    public PowerupsPanelController powerupsPanelController;
    public QuestsController questsController;

    private List<LetterTileController> tileList = new List<LetterTileController>();
    private List<LetterTileController> usedTileList = new List<LetterTileController>();
    private List<Vector3> positionsList;
    private GameController gameController;

    //analytics
    private float trialStartTime;

    public void SetGameController(GameController gameControllerSet)
    {
        gameController = gameControllerSet;
    }
    public void BeginTrial()
    {
        questsController.BeginTrial();
        InitializeLetterBoard();
        MoneyController.Instance.OnTrialBegin();
        purseController.Init(gameController.rVController.RequestAd);
        currentWordText.text = "";
        trialStartTime = Time.time;
        AnalyticsController.OnBeginTrial();

    }

    
    private void InitializeLetterBoard()
    {
        LetterBasket.Initialize();
        usedTileList.Clear();
        //destroy placeholder editor tiles
        foreach (Transform child in tileGroupParent)
        {
            GameObject.Destroy(child.gameObject);
        }
        CreateLetterBoard();
    }

    private void CreateLetterBoard()
    {
        tileList = new List<LetterTileController>();
        //spawn all tiles     
        for(int i = 0; i < 16; i++)
        {
            LetterTileController tile = Instantiate<LetterTileController>(tilePrefab, tileGroupParent);
            tile.diceIdx = i;
            string letterToSet = GetNewLetterForTileIdx(tile.diceIdx);
            tile.SetTileText(letterToSet);
            tile.pressedCallback = OnTilePressed;
            tile.TileEnter();
            tileList.Add(tile);
        }
        Invoke(HandleTrialEnterComplete, .6f);
    }

    private string GetNewLetterForTileIdx(int diceIdx)
    {
        string tileTextSet = LetterBasket.RollDiceAtIdx(diceIdx);
        List<string> options = LetterBasket.GetDiceOptionsAtIdx(diceIdx);
        List<string> requiredLetters = questsController.GetRequiredLetters();
        foreach (string letter in requiredLetters)
        {
            if (options.Contains(letter))
            {
                if (UnityEngine.Random.Range(0f, 1f) < .75f)
                {
                    tileTextSet = letter;
                    break;
                }
            }
        }
        return tileTextSet;
    }

    private void HandleTrialEnterComplete()
    {
        gameController.EnableInput();
    }

    private void StorePositions()
    {
        positionsList = new List<Vector3>();
        foreach(LetterTileController tile in tileList)
        {
            positionsList.Add(tile.transform.position);
        }
    }
    
    
    public void OnTilePressed(LetterTileController tileController)
    {
        if (!usedTileList.Contains(tileController))
        {
            tileController.SetTileUsed();
            usedTileList.Add(tileController);
            currentWordText.text += tileController.letterText.text;
        }
        else if(usedTileList.Count > 0 && usedTileList[usedTileList.Count-1] == tileController)
        {
            Backspace();
        } 

    }

    public void Backspace()
    {
        //MoneyController.AwardCoins(new List<Vector2>() { Vector2.zero }, 15); //Testing doobers - 
        if (usedTileList.Count > 0 && currentWordText.text.Length > 0)
        {
            int idx = usedTileList.Count-1;
            LetterTileController lastTile = usedTileList[idx];
            string lastLetterContribution = lastTile.letterText.text;
            currentWordText.text = currentWordText.text.Substring(0, currentWordText.text.Length - lastLetterContribution.Length);
            
            lastTile.RevertUsedTile();
            usedTileList.Remove(lastTile);
        }
    }
    
    public void ClearAllTiles()
    {
        int count = usedTileList.Count;
        for(int i = 0; i < count; i++)
        {
            Backspace();
        }
    }

    public void OnSubmitButtonPressed()
    {
        string word = currentWordText.text;
        bool wordHasBeenUsed = HasWordBeenUsed();
        bool wordIsInDictionary = IsWordInDictionary(word);
        bool wordIsLongEnough = word.Length >= 2;
        if (!wordIsLongEnough && word.Length != 0)
        {
            ToasterPanelController.ShowToaster("Words must have at least 2 letters");
        }
        bool isValid = !wordHasBeenUsed && wordIsInDictionary && wordIsLongEnough;
        if (isValid)
        {
            if (word.Length >= GameController.LONG_WORD_CHARACTER_REQUIREMENT)
            {
                //ToasterController.ShowLongWordToaster();
            }
            gameController.OnSubmitButtonPressed(word);
            Submit(word);
        } 
        else
        {
            //show rejected feedback
            ClearWord();
            RejectUsedTiles();
        }
        
        
    }

    private bool HasWordBeenUsed()
    {
        return false;
    }

    private bool IsWordInDictionary(string word)
    {
        bool exists = DictionaryController.ExistsInDictionary(word);
        return exists;
    }


    private void Submit(string word)
    {
        bool isWordForQuest = questsController.TrackCompletedWord(word);
        AnalyticsController.OnWordSubmitted(word, isWordForQuest);
        ClearTilesAndWord();        
    }

    private void ClearTilesAndWord()
    {
        CreateCoinsForLetters();
        ClearTiles();
        ClearWord();
    }

    private void CreateCoinsForLetters()
    {
        foreach(LetterTileController tile in usedTileList)
        {
            if (tile.hasCoinValue)
            {
                AnalyticsController.OnUseCoinLetter();
                MoneyController.Instance.OnCoinTileUsed();
                MoneyController.AwardCoins(tile.GetCoinPosition(), 1, 1);
            }
        }
    }

    private void ClearTiles()
    {
        ExitTiles(usedTileList);
        StartCoroutine(ReplaceUsedTiles());
        
    }

    private void ExitTiles( List<LetterTileController> tileList)
    {
        foreach(LetterTileController tile in tileList)
        {
            tile.TileExit();
        }
    }

    IEnumerator ReplaceUsedTiles()
    {
        yield return new WaitForSeconds (1f);

        foreach(LetterTileController tile in usedTileList)
        {
            string letterToSet = GetNewLetterForTileIdx(tile.diceIdx);
            tile.SetTileText(letterToSet);
            tile.TileEnter();
        }
        usedTileList[usedTileList.Count-1].SetEnterCallback(gameController.EnableInput);
        usedTileList.Clear();

        // yield return new WaitForSeconds(.6f);
        // gameController.EnableInput();
    }

    public void OnShuffleButtonPressed()
    {
        //if they have enough coins, remove coins, recycle board
        if (MoneyController.GetCurrentMoney() >= SHUFFLE_LETTERS_COST)
        {
            MoneyController.ChangeMoney(-SHUFFLE_LETTERS_COST);
            ShuffleTiles();
            AnalyticsController.OnUsePowerup(PowerupType.shuffle);
        }
        else
        {
            if (gameController.rVController.IsAdReady())
            {
                int defecit = SHUFFLE_LETTERS_COST - MoneyController.GetCurrentMoney();
                AnalyticsController.OnFailedPowerup(PowerupType.shuffle);
                gameController.rVController.watchRVPanelController.ShowNeedCoins(defecit, AdOfferSource.fillDefecitForShuffle);
            }
            else
            {
                Debug.LogError("Ad not available to watch, show toaster for need more coins");
                //show toaster for need more coins
            }
        }
    }
    
    public void ShuffleTiles()
    {
        if (positionsList == null)
        {
            StorePositions();
        }
        List<Vector3> targetPositionsList = new List<Vector3>();
        foreach(Vector3 position in positionsList)
        {
            targetPositionsList.Add(position);
        }
        int counter = 0;
        while(targetPositionsList.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, targetPositionsList.Count);
            Vector3 targetPos = targetPositionsList[idx];
            targetPositionsList.RemoveAt(idx);
            tileList[counter].transform.DOMove(targetPos, .2f);
            counter++;
        }
    }

    public void OnRecycleButtonPressed()
    {
        //if they have enough coins, remove coins, recycle board
        if (MoneyController.GetCurrentMoney() >= RECYCLE_LETTERS_COST)
        {
            MoneyController.ChangeMoney(-RECYCLE_LETTERS_COST);
            RecycleBoard();
            AnalyticsController.OnUsePowerup(PowerupType.recycle);
        }
        else
        {
            if (gameController.rVController.IsAdReady())
            {
                int defecit = RECYCLE_LETTERS_COST - MoneyController.GetCurrentMoney();
                AnalyticsController.OnFailedPowerup(PowerupType.recycle);
                gameController.rVController.watchRVPanelController.ShowNeedCoins(defecit, AdOfferSource.fillDefecitForShuffle);
            }
            else
            {
                ToasterPanelController.ShowToaster("You don't have enough coins for that!");
            }
        }        
    }

    public void RecycleBoard()
    {
        ClearWord();
        ExitTiles(tileList);
        MoneyController.Instance.ResetCurrentCoinLetterCount();
        //get rid of old tiles
        for(int i = 0; i < tileList.Count; i ++)
        {
            LetterTileController tile = tileList[i];
            Destroy(tile.gameObject);
        }
        InitializeLetterBoard();

    }

    private void RejectUsedTiles()
    {
        foreach(LetterTileController tile in usedTileList)
        {
            tile.PlayIncorrectAnimation();
        }
        usedTileList.Clear();
    }

    private void ClearWord()
    {
        currentWordText.text = "";
    }

    public string GetCurrentWord()
    {
        return currentWordText.text;
    }

    public PowerupsPanelController GetPowerupsPanel()
    {
        return powerupsPanelController;
    }

    public void HandleAllQuestsCompleted()
    {
        Invoke(ExitTrial, 5f);
    }

    private void ExitTrial()
    {
        AnalyticsController.OnCompleteTrial();
        gameController.EndTrial();
    }

    public Coroutine Invoke(Action action, float time)
    {
        return StartCoroutine(InvokeImpl(action, time));
    }

    private IEnumerator InvokeImpl(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

}
