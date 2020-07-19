using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the quests
//tracks the quests as they are progressed, updates view
//callback called when all quests are completed
public class QuestsController : MonoBehaviour
{
    public Transform questParent;
    public QuestItem exactLengthQuestPrefab;
    public QuestItem minimumLengthQuestPrefab;

    public TrialController trialController;

    private List<QuestItem> questItemList;
    

    private int completedQuestCount;
    private Dictionary<int, TrialData> trialLibrary;

    public void BeginTrial()
    {
        GenerateTrialLibrary();
        CreateQuests();
        completedQuestCount = 0;

    }

    private void CreateQuests()
    {
        int lastCompletedQuest = PlayerPrefsPro.GetInt("lastCompletedQuest", 0);
        //fetch quest data for trial
        int idx = lastCompletedQuest+1;
        if (idx >= trialLibrary.Count)
        {
            idx = 0;
            PlayerPrefsPro.SetInt("lastCompletedQuest", 0);
            PlayerPrefsPro.Save();
        }

        TrialData tData = trialLibrary[idx];

        //create quests from data
        questItemList = new List<QuestItem>();
        foreach(QuestData qData in tData.questList)
        {
            QuestItem qItem;
            switch(qData.questType)
            {
                case QuestType.exactLength:
                    qItem = Instantiate<QuestItem>(exactLengthQuestPrefab, questParent);
                    break;
                case QuestType.minimumLength:
                    qItem = Instantiate<QuestItem>(minimumLengthQuestPrefab, questParent);
                    break;
                default:
                    qItem = new QuestItem();
                    break;
            }
            qItem.Populate(qData.goalTitle, qData.targetCount, qData.goalNum);
            questItemList.Add(qItem);
            qItem.onQuestCompletedCallback.AddListener(OnQuestCompleted);
        }

        Canvas.ForceUpdateCanvases();
        float width = Screen.width;
        // float width = questParent.GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log("Quest wdith: " + width);
        float distance = width / questItemList.Count; 
        float offset;
        if (questItemList.Count % 2 != 0)
            offset = questItemList.Count/2 * distance;
        else
            offset = (questItemList.Count/2 - .5f ) * distance;
        for (int i = 0; i < questItemList.Count; i++)
        {
            float xPos = (i * distance) - offset;
            questItemList[i].transform.localPosition = new Vector3 (xPos, 0, 0);
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
        if (completedQuestCount == questItemList.Count)
        {
            HandleAllQuestsCompleted();
        }
    }

    public void HandleAllQuestsCompleted()
    {
        int lastCompletedQuest = PlayerPrefsPro.GetInt("lastCompletedQuest", 0);
        PlayerPrefsPro.SetInt("lastCompletedQuest", lastCompletedQuest+1);
        PlayerPrefsPro.Save();
        trialController.HandleAllQuestsCompleted();
    }

    public void GenerateTrialLibrary()
    {
        trialLibrary = new Dictionary<int, TrialData>();
        List<QuestData> qData;
        TrialData tData;
        
        //quest 1
        qData = new List<QuestData>();
        qData.Add(new QuestData( 3, 3, QuestType.exactLength, "3 letter words"));
        qData.Add(new QuestData( 4, 1, QuestType.exactLength, "4 letter words"));
        tData = new TrialData(0, qData);
        trialLibrary.Add(0, tData);

        //quest 2
        qData = new List<QuestData>();
        qData.Add(new QuestData( 2, 2, QuestType.exactLength, "2 letter words"));
        qData.Add(new QuestData( 3, 3, QuestType.exactLength, "3 letter words"));
        qData.Add(new QuestData( 4, 1, QuestType.exactLength, "4 letter words"));
        tData = new TrialData(1, qData);
        trialLibrary.Add(1, tData);
        
        //quest 3
        qData = new List<QuestData>();
        qData.Add(new QuestData( 3, 3, QuestType.minimumLength, "3+ letter words"));
        tData = new TrialData(2, qData);
        trialLibrary.Add(2, tData);

        //quest 4
        qData = new List<QuestData>();
        qData.Add(new QuestData( 4, 2, QuestType.exactLength, "4 letter words"));
        tData = new TrialData(3, qData);
        trialLibrary.Add(3, tData);
    }
}

public enum QuestType
{
    exactLength = 0,
    minimumLength = 1,

}

public class QuestData
{
    public string goalTitle;
    public int targetCount;
    public int goalNum;
    public QuestType questType;

    public QuestData (int targetCountSet, int goalNumSet, QuestType questTypeSet, string goalTitleSet)
    {
        goalTitle = goalTitleSet;
        targetCount = targetCountSet;
        goalNum  = goalNumSet;
        questType = questTypeSet;
    }
}

public class TrialData
{
    public int id;
    public List<QuestData> questList;

    public TrialData(int idSet, List<QuestData> questListSet)
    {
        id = idSet;
        questList = questListSet;
    }
}