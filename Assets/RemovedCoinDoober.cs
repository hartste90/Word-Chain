using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RemovedCoinDoober : MonoBehaviour
{
    public TextMeshProUGUI label;
    public CanvasGroup cg;

    public void Initialize(int amt, Vector2 origin)
    {
        transform.position = origin;
        label.text = "- " + amt.ToString();
        cg.alpha = 0f;

        cg.DOFade(1f, .1f);

        Sequence s = DOTween.Sequence();
        s.Append(cg.transform.DOMoveY(origin.y + 70f, 1f).SetEase(Ease.OutCirc));
        s.Append(cg.DOFade(0f, .1f));
        s.AppendCallback(() => { Destroy(gameObject); });
        s.Play();

    }
}
