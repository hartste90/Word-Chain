using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PowerupType
{
    NewBoard = 1,
}
public class PowerupsPanelController : MonoBehaviour
{

    void Start()
    {
        int numNewBoard = PlayerPrefs.GetInt("NewBoardPowerups", 0);
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

    }

    private void UsePowerup(PowerupType powerupType)
    {
        //STUB
    }
}
