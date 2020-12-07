using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates the quests
//tracks the quests as they are progressed, updates view
//callback called when all quests are completed

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
        //float width = Screen.width;
        //// float width = questParent.GetComponent<RectTransform>().sizeDelta.x;
        //Debug.Log("Quest wdith: " + width);
        //float distance = width / questItemList.Count; 
        //float offset;
        //if (questItemList.Count % 2 != 0)
        //    offset = questItemList.Count/2 * distance;
        //else
        //    offset = (questItemList.Count/2 - .5f ) * distance;
        //for (int i = 0; i < questItemList.Count; i++)
        //{
        //    float xPos = (i * distance) - offset;
        //    questItemList[i].transform.localPosition = new Vector3 (xPos, 0, 0);
        //}
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

    public bool TrackCompletedWord(string word)
    {
        PlayerPrefs.SetInt(AnalyticsKeys.words_this_session, PlayerPrefs.GetInt(AnalyticsKeys.words_this_session, 0) + 1);
        bool isWordForQuest = false;
        foreach (QuestItem quest in questItemList)
        {
            bool currentQuestNeedsWord = quest.AccountWord(word);
            isWordForQuest = isWordForQuest || currentQuestNeedsWord;
        }
        return isWordForQuest;
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

/*NOTE
*
    Safe letters to use for specific letter (have more than 3 occurances across tiles):
    All vowels, D, G, H, L, N, M, P, R, S*(easiest), T
*/

        GenerateTrialLibraryImpl();
        return;
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

    private void GenerateTrialLibraryImpl()
    {
        trialLibrary = new Dictionary<int, TrialData>();
        List<QuestData> qDataList = new List<QuestData>();

        for(int i = 0; i < 1000; i++)
        {
            //generate total difficulty for this trial
            int trialDifficulty = Random.Range(3, 10);
            qDataList.Clear();
            HashSet<QuestType> questList = new HashSet<QuestType>();
            while (qDataList.Count < 2 && trialDifficulty >= 0)
            {
                QuestData qData = CreateRandomQuest();
                if (!questList.Contains(qData.questType))
                { 
                    questList.Add(qData.questType);
                    qDataList.Add(qData);
                    trialDifficulty -= 3;
                }
            }
            //add to trial library
            TrialData tData = new TrialData(i, qDataList);
            trialLibrary.Add(i, tData);
        }
    }

    private QuestData CreateRandomQuest()
    {
        // int questDifficulty = Random.Range(Mathf.Min(trialDifficulty, 3), 5);
        // Debug.Log("Creating Quest of diff: " + questDifficulty);
        // trialDifficulty -= questDifficulty;
        // All vowels, D, G, H, L, N, M, P, R, S*(easiest), T
        List<string> specificLetters = new List<string>(){ "A", "E", "I", "O", "D", "G", "H", "L", "N", "M", "P", "R", "S", "T" };
        QuestType qType = (QuestType)Random.Range(0, 5);
        QuestData qData = new QuestData(qType, Random.Range(1, 4), Random.Range(2, 5));
        switch(qType)
        {
            case QuestType.exactLength:
                qData = new QuestData(qType, Random.Range(3, 10), Random.Range(2, 5));
                break;
            case QuestType.minimumLength:
                qData = new QuestData(qType, Random.Range(3, 10), Random.Range(3, 6));
                break;
            case QuestType.specificLetter:
                qData = new QuestData(qType, Random.Range(2, 5), 0, specificLetters[Random.Range(0, specificLetters.Count)]);
                break;
            case QuestType.vowelWord:
                qData = new QuestData(qType, Random.Range(4, 10), 0, "");
                break;
            case QuestType.twoVowelWord:
                qData = new QuestData(qType, Random.Range(4, 10), 0, "");
                break;
        }
        return qData;
    }

    public List<string> GetRequiredLetters()
    {
        List<string> requiredLetters = new List<string>();
        foreach(QuestItem quest in questItemList)
        {
            if (!quest.IsQuestComplete() && quest.GetQuestType() == QuestType.specificLetter)
            {
                requiredLetters.Add(quest.GetRequiredLetter());
            }
        }
        return requiredLetters;
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