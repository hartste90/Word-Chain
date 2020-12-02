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

    public CoinDooberController coinDooberPrefab;
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

    public static void AwardCoins(List<Vector2> screenPositions, int coinAmtToAdd)
    {
        //When give coins, send list of positions
        int coinValue = coinAmtToAdd / screenPositions.Count;
        int remainder = coinAmtToAdd % screenPositions.Count;
        for (int idx = 0; idx < screenPositions.Count; idx++)
        {
            Vector2 point = screenPositions[idx];
            CoinDooberController coin = Instantiate<CoinDooberController>(Instance.coinDooberPrefab, GameController.Instance.trialParent);
            int value = coinValue;
            if (idx == 0)
            {
                value += remainder;
            }
            coin.Init(value, point, Instance.OnCoinDooberHitPurse);
        }
    }

    public static void AwardCoins(Vector2 screenPosition, int coinAmtToAdd, int dooberNum)
    {
        if (dooberNum > coinAmtToAdd)
        {
            dooberNum = coinAmtToAdd;
        }
        if (dooberNum == 0)
        {
            dooberNum = 1;
        }
        int coinValue = coinAmtToAdd / dooberNum;
        int remainder = coinAmtToAdd % dooberNum;
        for (int idx = 0; idx < dooberNum; idx++)
        {
            CoinDooberController coin = Instantiate<CoinDooberController>(Instance.coinDooberPrefab, GameController.Instance.trialParent);
            int value = coinValue;
            if (idx == 0)
            {
                value += remainder;
            }
            coin.Init(value, screenPosition, Instance.OnCoinDooberHitPurse);
        }
    }

    public static Vector2 GetPurseScreenPosition()
    {
        return GameController.GetPurseScreenPosition();
    }

    private void OnCoinDooberHitPurse(int coinValue)
    {
        ChangeMoney(coinValue);
    }

    public static void SetMoney(int moneyToSet = 0)
    { 
        PlayerPrefs.SetInt(PLAYER_MONEY_KEY, moneyToSet);
        onMoneyChanged?.Invoke(moneyToSet);
    }


}
