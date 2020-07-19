using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using GameAnalyticsSDK;

public class TrialController : MonoBehaviour
{
    public TextMeshProUGUI currentWordText;

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
        InitializeLetterBoard();
        currentWordText.text = "";
        questsController.BeginTrial();
        trialStartTime = Time.time;

    }

    
    private void InitializeLetterBoard()
    {
        LetterBasket.Initialize();
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
            tile.SetTileText(LetterBasket.RollDiceAtIdx(tile.diceIdx));
            tile.pressedCallback = OnTilePressed;
            tile.TileEnter();
            tileList.Add(tile);
        }
        Invoke(HandleTrialEnterComplete, .6f);
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
        bool isValid = !wordHasBeenUsed && wordIsInDictionary;
        if (isValid)
        {
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
        questsController.TrackCompletedWord(word);
        ClearTilesAndWord();        
    }

    private void ClearTilesAndWord()
    {
        ClearTiles();
        ClearWord();
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
            tile.SetTileText(LetterBasket.RollDiceAtIdx(tile.diceIdx));
            tile.TileEnter();
        }
        usedTileList.Clear();

        yield return new WaitForSeconds(.6f);
        gameController.EnableInput();
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

    public void RecycleBoard()
    {
        ClearWord();
        ExitTiles(tileList);
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
        ExitTrial();
        GameAnalytics.NewDesignEvent ("trialComplete", Time.time-trialStartTime);
    }

    private void ExitTrial()
    {
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
