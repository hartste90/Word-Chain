using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class QuestItem : MonoBehaviour
{

    public TextMeshProUGUI goalNameText;
    public TextMeshProUGUI goalProgressText;

    public GameObject statusBackgroundObj;
    public GameObject statusCompleteObj;
    
    
    public UnityEvent onQuestCompletedCallback;

    protected int targetCount;
    protected int currentCount = 0;
    protected int goalNum;

    public virtual void AccountWord(string word)
    {
        UpdateGoalProgress();
    }

    public void Populate(string goalNameSet, int targetCounSet, int goalCountSet)
    {
        goalNum = goalCountSet;
        targetCount = targetCounSet;
        goalNameText.text = goalNameSet;
        UpdateGoalProgress();
    }

    private void CompleteQuest()
    {
        ShowComplete();
        onQuestCompletedCallback.Invoke();
    }
    public void ShowComplete()
    {
        statusBackgroundObj.SetActive(false);
        statusCompleteObj.SetActive(true);
    }

    private void UpdateGoalProgress()
    {
        if (currentCount == goalNum)
        {
            CompleteQuest();
        }
        else if (statusBackgroundObj.activeInHierarchy)
        {
            goalProgressText.text = (goalNum-currentCount).ToString() + " left";
        }
    }
 
}
