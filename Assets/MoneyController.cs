using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyController : MonoBehaviour
{

    const string PLAYER_MONEY_KEY = "player_money_amt";
    #region singleton
    private static MoneyController instance;
    public static MoneyController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MoneyController>();
                if (instance == null)
                {
                    Debug.LogError("No MoneyController was found in this scene. Make sure you place one in the scene.");
                    return instance;
                }
            }
            return instance;
        }
    }
    #endregion


    public static UnityAction<int> onMoneyChanged;


    public static int GetCurrentMoney()
    {
        return PlayerPrefs.GetInt(PLAYER_MONEY_KEY, 200);
    }

    public static void ChangeMoney(int moneyToAdd)
    {
        int currentMoney = GetCurrentMoney();
        currentMoney += moneyToAdd;
        PlayerPrefs.SetInt(PLAYER_MONEY_KEY, currentMoney);
        onMoneyChanged?.Invoke(currentMoney);
    }

    public static void SetMoney(int moneyToSet = 0)
    { 
        PlayerPrefs.SetInt(PLAYER_MONEY_KEY, moneyToSet);
        onMoneyChanged?.Invoke(moneyToSet);
    }


}
