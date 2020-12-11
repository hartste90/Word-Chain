using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyController : MonoBehaviour
{
    const int DOOBER_COUNT_MAX = 50;
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
    public RemovedCoinDoober removedCoinDooberPrefab;
    public static UnityAction<int> onMoneyChanged;
    public static UnityAction onCoinAnimationComplete;

    private int currentCoinLetterCount = 0;
    private List<string> coinLetters = new List<string>() { "Z", "X", "Qu", "V", "B", "J", "K", "W" };


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

    public static void AddMoney(int moneyToAdd)
    {
        ChangeMoney(moneyToAdd);
    }

    public static void RemoveMoney(int moneyToRemove, bool shouldVisualize = false, Vector2 origin = new Vector2())
    {
        ChangeMoney(-moneyToRemove);
        if (shouldVisualize)
        {
            RemovedCoinDoober doober = Instantiate<RemovedCoinDoober>(Instance.removedCoinDooberPrefab, GameController.Instance.trialParent);
            doober.Initialize(moneyToRemove, origin);
        }
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
        dooberNum = Mathf.Min(dooberNum, DOOBER_COUNT_MAX);
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
        SoundController.PlayAwardCoins();
    }

    public static Vector2 GetPurseScreenPosition()
    {
        return GameController.GetPurseScreenPosition();
    }

    private void OnCoinDooberHitPurse(int coinValue)
    {
        AddMoney(coinValue);
        onCoinAnimationComplete?.Invoke();

    }
    public static void SetMoney(int moneyToSet = 0)
    { 
        PlayerPrefs.SetInt(PLAYER_MONEY_KEY, moneyToSet);
        onMoneyChanged?.Invoke(moneyToSet);
    }

    public static bool ShouldBeCoinLetter (string letterText)
    {
        //could be coin if (difficult letter / havent had one in a while)
        Debug.Log("Current coin letters: " + Instance.currentCoinLetterCount);
        if (Instance.coinLetters.Contains(letterText) && Instance.currentCoinLetterCount < 2)
        {
            Instance.currentCoinLetterCount++;
            return true;
        }
        return false;
    }

    public void ResetCurrentCoinLetterCount()
    {
        currentCoinLetterCount = 0;
    }

    public void OnCoinTileUsed()
    {
        currentCoinLetterCount--;
    }

    public void OnTrialBegin()
    {
        ResetCurrentCoinLetterCount();
    }



}
