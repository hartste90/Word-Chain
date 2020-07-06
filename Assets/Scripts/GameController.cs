using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    


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


    private List<LetterTileController> usedTileList = new List<LetterTileController>();
    
    void Start()
    {
        LetterBasket.Initialize();
        currentWordText.text = "";

        //destroy placeholder editor tiles
        foreach (Transform child in tileGroupParent)
        {
            GameObject.Destroy(child.gameObject);
        }
        //spawn all tiles     
        for(int i = 0; i < 16; i++)
        {
            LetterTileController tile = Instantiate<LetterTileController>(tilePrefab, tileGroupParent);
            tile.diceIdx = i;
            tile.SetTileText(LetterBasket.RollDiceAtIdx(tile.diceIdx));
            tile.pressedCallback = OnTilePressed;
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
        if (usedTileList.Count > 0)
        {
            int idx = usedTileList.Count-1;
            LetterTileController lastTile = usedTileList[idx];
            string lastLetterContribution = lastTile.letterText.text;
            currentWordText.text = currentWordText.text.Substring(0, currentWordText.text.Length - lastLetterContribution.Length);
            
            lastTile.SetTileAvailable();
            usedTileList.Remove(lastTile);
        }
    }

    public void OnSubmitButtonPressed()
    {
        Submit();
    }

    private void Submit()
    {
        //clear current word
        currentWordText.text = "";
        //used tiles -->
            //roll letter
            //set available
        foreach(LetterTileController tile in usedTileList)
        {
            tile.SetTileText(LetterBasket.RollDiceAtIdx(tile.diceIdx));
            tile.SetTileAvailable();
        }
        usedTileList.Clear();
    }

}
