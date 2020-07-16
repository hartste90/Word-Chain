using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the quests
//tracks the quests as they are progressed, updates view
//callback called when all quests are completed
public class QuestsController : MonoBehaviour
{
    public Transform questParent1;
    public Transform questParent2;
    public Transform questParent3;
    public QuestItem exactLengthQuestPrefab;
    public QuestItem minimumLengthQuestPrefab;

    public TrialController trialController;

    private List<QuestItem> questItemList;
    

    private int completedQuestCount;

    public void BeginTrial()
    {
        CreateQuests();
        completedQuestCount = 0;

    }

    private void CreateQuests()
    {
        questItemList = new List<QuestItem>();
        QuestItem quest4 = Instantiate<QuestItem>(exactLengthQuestPrefab, questParent1);
        QuestItem quest5 = Instantiate<QuestItem>(exactLengthQuestPrefab, questParent2);
        QuestItem quest6 = Instantiate<QuestItem>(exactLengthQuestPrefab, questParent3);
        quest4.Populate("4 letter words", 4, 2);
        quest5.Populate("5 letter words", 5, 1);
        quest6.Populate("6 letter words", 6, 1);
        questItemList.Add(quest4);
        questItemList.Add(quest5);
        questItemList.Add(quest6);
        foreach(QuestItem quest in questItemList)
        {
            quest.onQuestCompletedCallback.AddListener(OnQuestCompleted);
        }
    }

    public void TrackCompletedWord(string word)
    {
        foreach (QuestItem quest in questItemList)
        {
            quest.AccountWord(word);
        }
    }

    public void OnQuestCompleted()
    {
        completedQuestCount++;
        if (completedQuestCount == 3)
        {
            HandleAllQuestsCompleted();
        }
    }

    public void HandleAllQuestsCompleted()
    {
        // trialController.HandleAllQuestsCompleted();
        trialController.HandleAllQuestsCompleted();
    }
}
