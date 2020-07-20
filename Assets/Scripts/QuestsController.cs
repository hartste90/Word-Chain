using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the quests
//tracks the quests as they are progressed, updates view
//callback called when all quests are completed
/*NOTE
*
    Safe letters to use for specific letter (have more than 3 occurances across tiles):
    All vowels, D, G, H, L, N, M, P, R, S*(easiest), T
*/
public class QuestsController : MonoBehaviour
{
    public Transform questParent;
    public QuestItem questItemPrefab;

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
            CreateQuestFromData(qData);
            
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

    private void CreateQuestFromData(QuestData qData)
    {
            QuestItem qItem = Instantiate<QuestItem>(questItemPrefab, questParent);
            switch(qData.questType)
            {
                case QuestType.exactLength:
                    qItem.SetExactLengthType(qData.wordLengthTarget);
                    break;
                case QuestType.minimumLength:
                    qItem.SetMinimumLengthType(qData.wordLengthTarget);
                    break;
                case QuestType.specificLetter:
                    qItem.SetSpecificLetterType(qData.requiredLetter);
                    break;
                case QuestType.vowelWord:
                    qItem.SetVowelType();
                    break;
                case QuestType.twoVowelWord:
                    qItem.SetTwoVowelType();
                    break;
            }
            qItem.SetTotalWords(qData.questTotalWords);
            questItemList.Add(qItem);
            qItem.onQuestCompletedCallback.AddListener(OnQuestCompleted);
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
        qData.Add(new QuestData( QuestType.exactLength, 3, 3));
        qData.Add(new QuestData( QuestType.exactLength, 1, 4));
        qData.Add(new QuestData( QuestType.specificLetter, 1, 0, "T"));
        tData = new TrialData(0, qData);
        trialLibrary.Add(0, tData);

        //quest 2
        qData = new List<QuestData>();
        qData.Add(new QuestData( QuestType.exactLength, 3, 2));
        qData.Add(new QuestData( QuestType.exactLength, 2, 3));
        qData.Add(new QuestData( QuestType.exactLength, 1, 4));
        tData = new TrialData(1, qData);
        trialLibrary.Add(1, tData);
        
        //quest 3
        qData = new List<QuestData>();
        qData.Add(new QuestData( QuestType.minimumLength, 2, 4));
        qData.Add(new QuestData( QuestType.specificLetter, 2, 0, "P"));
        qData.Add(new QuestData( QuestType.vowelWord, 2));


        tData = new TrialData(2, qData);
        trialLibrary.Add(2, tData);

        //quest 4
        qData = new List<QuestData>();
        qData.Add(new QuestData( QuestType.specificLetter, 3, 0, "S"));
        qData.Add(new QuestData( QuestType.twoVowelWord, 2));
        tData = new TrialData(3, qData);
        trialLibrary.Add(3, tData);
    }
}

public enum QuestType
{
    exactLength = 0,
    minimumLength = 1,
    specificLetter = 2,

    vowelWord = 3,
    twoVowelWord = 4

}

public class QuestData
{
    public int wordLengthTarget;
    public int questTotalWords;
    public string requiredLetter;
    public QuestType questType;

    public QuestData (QuestType questTypeSet, int questTotalWordsSet, int wordLengthTargetSet = 0, string requiredLetterSet = "")
    {
        wordLengthTarget = wordLengthTargetSet;
        questTotalWords  = questTotalWordsSet;
        questType = questTypeSet;
        requiredLetter = requiredLetterSet;
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