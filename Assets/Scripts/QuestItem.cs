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


    public virtual void AccountWord(string word)
    {
        switch(questType)
        {
            case QuestType.exactLength:
                AccountExactLengthQuest(word);
                break;
            case QuestType.minimumLength:
                AccountMinimumLengthQuest(word);
                break;
            case QuestType.specificLetter:
                AccountSpecificLetterQuest(word);
                break;
            case QuestType.vowelWord:
                AccountVowelQuest(word);
                break;
            case QuestType.twoVowelWord:
                AccountTwoVowelQuest(word);
                break;
        }
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

    private void AccountExactLengthQuest(string word)
    {
        if (word.Length == wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
        }
    }

    private void AccountMinimumLengthQuest(string word)
    {
        if (word.Length >= wordLengthTarget)
        {
            currentCount++;
            MarkProgressMade();
        }
    }

    private void AccountSpecificLetterQuest(string word)
    {

        if (!string.IsNullOrEmpty(requiredLetter) && word.Contains(requiredLetter))
        {
            currentCount++;
            MarkProgressMade();
        }
    }

    private void AccountVowelQuest(string word)
    {

        int vowelIdx = ContainsVowel(word);
        if (vowelIdx != -1)
        {
            string sub = word.Remove(vowelIdx, 1);
            if (ContainsVowel(sub) == -1)
            {
                currentCount++;
                MarkProgressMade();
            }
        }
    }

    private void AccountTwoVowelQuest(string word)
    {
        int vowelIdx = ContainsVowel(word);
        if (vowelIdx != -1)
        {
            string sub = word.Remove(vowelIdx, 1);
            vowelIdx = ContainsVowel(sub);
            if (vowelIdx != -1)
            {
                sub = sub.Remove(vowelIdx, 1);
                if (ContainsVowel(sub) == -1)
                {
                    currentCount++;
                    MarkProgressMade();
                }
            }
        }
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
        else if (word.Contains("Y"))
        {
            return word.IndexOf("Y");
        }
        else
        {
            return -1;
        }
    }


    public virtual void MarkProgressMade()
    {
        if (currentCount == questTotalWords)
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
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 90f, .5f).SetEase(Ease.InSine));
        // seq.AppendCallback(UpdateText);
        // seq.AppendCallback(() => goalProgressText.transform.localRotation = Quaternion.Euler(0,-90f,0));
        // seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 0f, .5f).SetEase(Ease.OutSine));
        seq.Append(questCompleteFillImage.DOFillAmount(1f, .2f));
        seq.Append(goalProgressText.DOFade(0, .1f));
        seq.AppendCallback(() => statusCompleteObj.SetActive(true));
        seq.Append(statusCompleteObj.transform.DOShakeScale(.2f));
        seq.Append(transform.DOScale(Vector3.one, .2f));
        seq.OnComplete(AwardCoins);
        seq.Play();
    }

    public void AwardCoins()
    {
        int reward = questTotalWords;
        MoneyController.AwardCoins(transform.position, reward, reward - 1);
    }
 
}
