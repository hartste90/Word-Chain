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

    protected int targetCount;
    protected int currentCount = 0;
    protected int goalNum;
    private float goalCelebrationDelay = 1f;

    public virtual void AccountWord(string word)
    {
        // UpdateGoalProgress();
    }

    public virtual void MarkProgressMade()
    {
        if (currentCount == goalNum)
        {
            PlayQuestCompletedAnimation();
            onQuestCompletedCallback?.Invoke();
        }
        else if (currentCount < goalNum)
        {
            PlayProgressMadeAnimation();
        }
    }

    public void Populate(string goalNameSet, int targetCounSet, int goalCountSet)
    {
        goalNum = goalCountSet;
        targetCount = targetCounSet;
        goalNameText.text = goalNameSet;
        UpdateText();
    }

    private void UpdateText()
    {
        goalProgressText.text = (goalNum-currentCount).ToString() + " left";
    }

    public void PlayProgressMadeAnimation()
    {
        goalProgressText.transform.localRotation = Quaternion.identity;
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(goalCelebrationDelay);
        seq.Append(transform.DOScale(Vector3.one * 1.3f, .2f));
        float fillAmt = (float)currentCount/(float)goalNum + (1f/(float) goalNum / 4f);
        seq.Append(fillImage.DOFillAmount(fillAmt, .2f));
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 90f, .5f).SetEase(Ease.InSine));
        seq.AppendCallback(UpdateText);
        seq.AppendCallback(() => goalProgressText.transform.localRotation = Quaternion.Euler(0,-90f,0));
        seq.Append(goalProgressText.transform.DOLocalRotate(Vector3.up * 0f, .5f).SetEase(Ease.OutSine));
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
        seq.Play();
    }
 
}
