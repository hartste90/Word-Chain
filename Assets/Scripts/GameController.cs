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

    public TextMeshProUGUI currentWordText;

    public LetterTileController tilePrefab;

    public Transform tileGroupParent;

    public BackgroundController backgroundController;


    private List<LetterTileController> tileList = new List<LetterTileController>();
    private List<LetterTileController> usedTileList = new List<LetterTileController>();
    private List<Vector3> positionsList;

    void Start()
    {
        InitializeSDKs();
        InitializeDictionary();
        InitializeLetterBoard();
        currentWordText.text = "";

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

    public void OnBackspaceButtonPressed()
    {
        Backspace();
    }

    private void Backspace()
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

    public void OnClearAllButtonPressed()
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
        CreateInBackground(word);
        ClearTilesAndWord();        
    }

    private void ClearTilesAndWord()
    {
        ClearTiles();
        ClearWord();
    }

    private void ClearTiles()
    {
        foreach(LetterTileController tile in usedTileList)
        {
            tile.SetTileText(LetterBasket.RollDiceAtIdx(tile.diceIdx));
            tile.TileEnter();
        }
        usedTileList.Clear();
    }

    private void CreateInBackground(string word)
    {
        backgroundController.SpawnWord(word);
    }

    public void OnShuffleButtonPressed()
    {
        ShuffleTiles();
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
            int idx = Random.Range(0, targetPositionsList.Count);
            Vector3 targetPos = targetPositionsList[idx];
            targetPositionsList.RemoveAt(idx);
            tileList[counter].transform.DOMove(targetPos, .2f);
            counter++;
        }
    }

    public void RecycleBoard()
    {
        ClearWord();
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

}
