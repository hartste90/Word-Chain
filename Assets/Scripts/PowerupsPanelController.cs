using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum CurrencyAmount
{
    NewBoard = 1,
    CoinsSmall = 2,
    CoinsMedium = 3,
    CoinsLarge = 4,
    CoinsHuge = 5
}
public class PowerupsPanelController : MonoBehaviour
{
    public PowerupButton newBoardButton;

    void Start()
    {
        int numNewBoard = PlayerPrefs.GetInt("NewBoardPowerups", 0);
        newBoardButton.SetCounterText(numNewBoard.ToString());
    }
    public void OnNewBoardButtonPressed()
    {
        //if available
        int numNewBoard = PlayerPrefs.GetInt("NewBoardPowerups", 0);
        if (numNewBoard > 0)
        {
            numNewBoard--;
            PlayerPrefs.SetInt("NewBoardPowerups", numNewBoard);
            newBoardButton.SetCounterText(numNewBoard.ToString());

            UsePowerup(CurrencyAmount.NewBoard);
        }
        else
        {
            newBoardButton.PlayUnavailableAnimation();
        }
            
        //if none avaialable
            //STUB: show not available message
    }

    public void AddNewBoardPowerup()
    {
        int numNewBoard = 1 + PlayerPrefs.GetInt("NewBoardPowerups", 0);
        PlayerPrefs.SetInt("NewBoardPowerups", numNewBoard);
        // newBoardButton.SetCounterText(numNewBoard.ToString());
        newBoardButton.PlayAddCounterAnimation();

    }

    private void UsePowerup(CurrencyAmount powerupType)
    {
        if (powerupType == CurrencyAmount.NewBoard)
        {
            //GameController.Instance.RecycleBoard();
        }
    }
}
