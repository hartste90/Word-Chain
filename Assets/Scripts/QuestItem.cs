using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class QuestItem : MonoBehaviour
{

    public TextMeshProUGUI goalNameText;
    public TextMeshProUGUI goalProgressText;

    public GameObject statusBackgroundObj;
    public GameObject statusCompleteObj;

    public Image fillImage;
    public Image questCompleteFillImage;
    
    
    public UnityEvent onQuestCompletedCallback;

    protected int wordLengthTarget;
    protected string requiredLetter;
    protected int currentCount = 0;
    protected int questTotalWords;
    private QuestType questType;
    private float goalCelebrationDelay = 1f;


    public QuestType GetQuestType()
    {
        return questType;
    }

    public string GetRequiredLetter()
    {
        if (GetQuestType() == QuestType.specificLetter && !string.IsNullOrEmpty(requiredLetter))
        {
            return requiredLetter;
        }
        else
        {
            return "";
        }
    }

    public int GetCurrentCount()
    {
        return currentCount;
    }

    public int GetQuestTotalWords()
    {
        return questTotalWords;
    }

    public bool IsQuestComplete()
    {
        return currentCount >= questTotalWords;
    }

    public virtual bool AccountWord(string word)
    {
        bool wordWasValid = false;
        switch(questType)
        {
            case QuestType.exactLength:
                wordWasValid = AccountExactLengthQuest(word);
                break;
            case QuestType.minimumLength:
                wordWasValid = AccountMinimumLengthQuest(word);
                break;
            case QuestType.specificLetter:
                wordWasValid = AccountSpecificLetterQuest(word);
                break;
            case QuestType.vowelWord:
                wordWasValid = AccountVowelQuest(word);
                break;
            case QuestType.twoVowelWord:
                wordWasValid = AccountTwoVowelQuest(word);
                break;
        }
        return wordWasValid;
    }

    public void SetTotalWords(int totalSet)
    {
        questTotalWords = totalSet;
        UpdateText();
    }
    public void SetExactLengthType(int targetLengthSet)
    {
        questType = QuestType.exactLength;
        wordLengthTarget = targetLengthSet;
        goalNameText.text = targetLengthSet + " letter words";

    }

    public void SetMinimumLengthType(int targetLengthSet)
    {
        questType = QuestType.minimumLength;
        wordLengthTarget = targetLengthSet;
        goalNameText.text = targetLengthSet + "+ letter words";
    }

    public void SetSpecificLetterType(string letterRequiredSet)
    {
        questType = QuestType.specificLetter;
        requiredLetter = letterRequiredSet;
        goalNameText.text = "Use \"" + letterRequiredSet + "\"";
    }

    public void SetVowelType()
    {
        questType = QuestType.vowelWord;
        goalNameText.text = "1 vowel words";
    }

    public void SetTwoVowelType()
    {
        questType = QuestType.twoVowelWord;
        goalNameText.text = "2 vowel words";
    }

    private bool AccountExactLengthQuest(string word)
    {
        if (word.Length == wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
            return true;
        }
        return false;
    }

    private bool AccountMinimumLengthQuest(string word)
    {
        if (word.Length >= wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
            return true;
        }
        return false;
    }

    private bool AccountSpecificLetterQuest(string word)
    {

        if (!string.IsNullOrEmpty(requiredLetter) && word.Contains(requiredLetter))
        {
            currentCount++;
            MarkProgressMade();
            return true;
        }
        return false;
    }

    private bool AccountVowelQuest(string word)
    {

        int vowelIdx = ContainsVowel(word);
        if (vowelIdx != -1)
        {
            string sub = RemoveFirstVowel(word);
            if (ContainsOnlyOptionalVowels(sub))
            //if (ContainsVowel(sub) == -1)
            {
                currentCount++;
                MarkProgressMade();
                return true;
            }
        }
        return false;
    }

    private bool AccountTwoVowelQuest(string word)
    {
        int vowelIdx = ContainsVowel(word);
        if (vowelIdx != -1)
        {
            string sub = RemoveFirstVowel(word);
            vowelIdx = ContainsVowel(sub);
            if (vowelIdx != -1)
            {
                sub = RemoveFirstVowel(sub);
                if (ContainsOnlyOptionalVowels(sub))
                {
                    currentCount++;
                    MarkProgressMade();
                    return true;
                }
            }
        }
        return false;
    }

    private int ContainsVowel(string word)
    {
        if (word.Contains("A"))
        {
            return word.IndexOf("A");
        } 
        else if (word.Contains("E"))
        {
            return word.IndexOf("E");
        } 
        else if (word.Contains("I"))
        {
            return word.IndexOf("I");
        } 
        else if (word.Contains("O"))
        {
            return word.IndexOf("O");
        } 
        else if (word.Contains("U"))
        {
            return word.IndexOf("U");
        } 
        else if (word.Contains("Qu"))
        {
            return word.IndexOf("Qu");
        }
        else if (word.Contains("Y"))
        {
            return word.IndexOf("Y");
        }
        else
        {
            return -1;
        }
    }

    private bool ContainsOnlyOptionalVowels(string word)
    {
        if (word.Contains("A"))
        {
            return false;
        }
        else if (word.Contains("E"))
        {
            return false;
        }
        else if (word.Contains("I"))
        {
            return false;
        }
        else if (word.Contains("O"))
        {
            return false;
        }
        else if (word.Contains("U"))
        {
            return false;
        }
        else if (word.Contains("Qu"))
        {
            return false;
        }
        return true;
    }

    private string RemoveFirstVowel(string word)
    {
        //handle special case for Qu being vowel
        int idx = ContainsVowel(word);
        if (word[idx] == 'Q')
        {
            word = word.Remove(idx, 2);
        }
        else
        {
            word = word.Remove(idx, 1);
        }
        return word;
    }


    public virtual void MarkProgressMade()
    {
        if (IsQuestComplete())
        {
            PlayQuestCompletedAnimation();
            onQuestCompletedCallback?.Invoke();
        }
        else if (currentCount < questTotalWords)
        {
            PlayProgressMadeAnimation();
        }
    }

    public void Populate(string goalNameSet, int targetCounSet, int goalCountSet)
    {
        questTotalWords = goalCountSet;
        wordLengthTarget = targetCounSet;
        goalNameText.text = goalNameSet;
        UpdateText();
    }

    private void UpdateText()
    {
        goalProgressText.text = (questTotalWords-currentCount).ToString() + " more";
    }

    public void PlayProgressMadeAnimation()
    {
        goalProgressText.transform.localRotation = Quaternion.identity;
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(goalCelebrationDelay);
        seq.Append(transform.DOScale(Vector3.one * 1.3f, .2f));
        float fillAmt = (float)currentCount/(float)questTotalWords + (1f/(float) questTotalWords / 4f);
        seq.Append(fillImage.DOFillAmount(fillAmt, .2f));
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 90f, .25f).SetEase(Ease.InSine));
        seq.AppendCallback(UpdateText);
        seq.AppendCallback(() => goalProgressText.transform.localRotation = Quaternion.Euler(0,-90f,0));
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 0f, .25f).SetEase(Ease.OutSine));
        seq.Append(transform.DOScale(Vector3.one, .2f));
        seq.Play();
    }

    public void PlayQuestCompletedAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(goalCelebrationDelay);
        seq.Append(transform.DOScale(Vector3.one * 1.3f, .2f));
        float fillAmt = 1f;
        seq.Append(fillImage.DOFillAmount(fillAmt, .2f));
        seq.AppendCallback(PlayQuestCompleteSound);
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 90f, .5f).SetEase(Ease.InSine));
        seq.Append(questCompleteFillImage.DOFillAmount(1f, .2f));
        seq.Append(goalProgressText.DOFade(0, .1f));
        seq.AppendCallback(() => statusCompleteObj.SetActive(true));
        seq.Append(statusCompleteObj.transform.DOShakeScale(.2f));
        seq.Append(transform.DOScale(Vector3.one, .2f));
        seq.OnComplete(AwardCoins);
        seq.Play();

    }

    private void PlayQuestCompleteSound()
    {
        SoundController.PlayCompleteQuest();
    }

    public void AwardCoins()
    {
        int reward = questTotalWords * 2;
        MoneyController.AwardCoins(transform.position, reward, questTotalWords);
    }
 
}
