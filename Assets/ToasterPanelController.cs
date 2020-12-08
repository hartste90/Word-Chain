using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ToasterPanelController : MonoBehaviour
{

    #region singleton
    private static ToasterPanelController instance;
    public static ToasterPanelController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ToasterPanelController>();
                if (instance == null)
                {
                    Debug.LogError("No ToasterPanelController was found in this scene. Make sure you place one in the scene.");
                    return instance;
                }
            }
            return instance;
        }
    }
    #endregion


    public TextMeshProUGUI panelText;
    private List<string> messageQueue = new List<string>();

    public bool isAnimating = false;

    public static void ShowToaster(string messageSet)
    {
        if (Instance.messageQueue.Contains(messageSet) == false)
        {
            Instance.messageQueue.Add(messageSet);
        }
    }

    private void Update()
    {
        if (!isAnimating && messageQueue.Count > 0)
        {
            ShowMessage(messageQueue[0]);
        }

    }

    private void ShowMessage(string messageSet)
    {
        isAnimating = true;
        panelText.text = messageSet;

        transform.localPosition = Vector3.up * 900;
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOLocalMoveY(500f, .4f).SetEase(Ease.OutBack));
        s.AppendInterval(2f);
        s.AppendCallback(PlayMessageOutro);
        s.Play();

    }

    private void PlayMessageOutro()
    {
        transform.DOLocalMoveY(900, .2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            messageQueue.RemoveAt(0);
            isAnimating = false;
        });
    }

}
