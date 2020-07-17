using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrialEndPanelAnimationCtrl : MonoBehaviour
{

    [SerializeField] Image scrim;
    [SerializeField] Image title;
    [SerializeField] RectTransform nextPanel;
    [SerializeField] CanvasGroup content;
    public void PlayTrialCompleteEnterAnimation()
    {
        Color finalColor = scrim.color;
        scrim.color = Color.clear;
        scrim.DOColor(finalColor, .5f);

        title.transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(title.transform.DOScale(Vector3.one, 1f));
        seq.Append(title.transform.DOPunchScale(Vector3.one * .05f, .3f).SetEase(Ease.OutElastic));
        seq.OnComplete(ShowPanel);
        seq.Play();

        nextPanel.localScale = Vector3.zero;
        content.alpha = 0f;
        

    }

    private void ShowPanel()
    {
        nextPanel.localScale = Vector3.one * .1f;
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(nextPanel.DOScaleX(1f, .4f));
        panelSeq.Append(nextPanel.DOScaleY(1f, .4f));
        panelSeq.Append(content.DOFade(1f, .2f));
        panelSeq.Play();
    }
}
