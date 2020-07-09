using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum PowerupType
{
    NewBoard = 1,
}
public class PowerupsPanelController : MonoBehaviour
{
    public TextMeshProUGUI newBoardPowerupText;

    void Start()
    {
        int numNewBoard = PlayerPrefs.GetInt("NewBoardPowerups", 0);
        newBoardPowerupText.text = numNewBoard.ToString();
    }
    public void OnNewBoardButtonPressed()
    {
        //if available
            UsePowerup(PowerupType.NewBoard);
        //if none avaialable
            //STUB: show not available message
    }

    public void AddNewBoardPowerup()
    {
        int numNewBoard = 1 + PlayerPrefs.GetInt("NewBoardPowerups", 0);
        PlayerPrefs.SetInt("NewBoardPowerups", numNewBoard);
        newBoardPowerupText.text = numNewBoard.ToString();

    }

    private void UsePowerup(PowerupType powerupType)
    {
        //STUB
    }
}
