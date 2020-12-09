using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
public class TutorialItem : MonoBehaviour
{
    private CanvasGroup cg;
    private UnityAction onAdvanceCallback;

    public Transform highlightCenter;
    public Transform mask;
    public CanvasGroup textPanel;

    private void Init()
    {
        cg = GetComponent<CanvasGroup>();    
    }

    public void Show(UnityAction callback)
    {
        Init();
        onAdvanceCallback = callback;
        gameObject.SetActive(true);
        PlayShowAnimation();

    }

    private void PlayShowAnimation()
    {
        cg.alpha = 0f;
        mask.localScale = Vector3.one * 5f;
        mask.localRotation = Quaternion.Euler(Vector3.forward * -45f);
        textPanel.alpha = 0f;

                
        mask.DOScale(Vector3.one, 1.7f).SetEase(Ease.OutBack);
        mask.DOLocalRotate(Vector3.zero, 1.5f, RotateMode.FastBeyond360);
        cg.DOFade(1, 2f).OnComplete(ShowText);
    }

    private void ShowText()
    {
        textPanel.DOFade(1f, .5f);
    }

    public void Hide()
    {
        cg.DOFade(0, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        
    }

    public void OnAdvanceButtonPressed()
    {
        onAdvanceCallback?.Invoke();
        SoundController.PlayUITap();
    }
}