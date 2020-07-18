using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class MovingRVButton : MonoBehaviour
{

    public float bubbleSpeed = .10f;
    public Image bonusTypeImage;

    public Sprite newBoardSprite;


    private float maxLifeTime = 50f;
    private float currentLifeTime;
    private float birthTime;
    private UnityEvent onPressedCallback = new UnityEvent();




    public void Initialize(PowerupType powerupType, UnityAction pressedCallbackSet)
    {
        onPressedCallback.AddListener(pressedCallbackSet);
        birthTime = Time.time;
        SetImage(powerupType);
        PlayBubbleInAnimation();
        LerpToScreenSpot();
    }

    private void OnFinishedLerping()
    {
        currentLifeTime = Time.time - birthTime;
        //if total time is up, animate out
        if (currentLifeTime > maxLifeTime)
        {
            PlayBubbleOutAnimation();
        }
        //else LerpToScreenSpot
        else
        {
            PlayBubbleBounceAnimation();
            LerpToScreenSpot();
        }
    }

    private void LerpToScreenSpot()
    {
        //get random spot onscreen
        Vector3 randomPosition = GetRandomScreenPoint();
        transform.DOMove(randomPosition, (randomPosition-transform.position).magnitude / bubbleSpeed).SetEase(Ease.InOutSine).OnComplete(OnFinishedLerping);
    }

    private Vector3 GetRandomScreenPoint()
    {
        float randomX = Random.Range(.1f, .9f);
        float randomY = Random.Range(.1f, .9f);
        Vector2 randomPositionOnScreen = Camera.main.ViewportToScreenPoint(new Vector2(randomX, randomY));
        return new Vector3(randomPositionOnScreen.x, randomPositionOnScreen.y, 0);
    }

    private void PlayBubbleInAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .3f);
    }

    private void PlayBubbleOutAnimation()
    {
        transform.DOScale(Vector3.zero, 1f).OnComplete(KillSelf);
    }

    private void PlayBubbleBounceAnimation()
    {
        transform.DOShakeScale( 1.2f, .2f, 5);
    }

    public void OnButtonPressed()
    {
        PlayBubbleOutAnimation();
        onPressedCallback?.Invoke();
    }

    private void SetImage(PowerupType powerupType)
    {
        switch(powerupType)
        {
            case PowerupType.NewBoard:
                bonusTypeImage.sprite = newBoardSprite;
                break;
        }
    }

    private void KillSelf()
    {
        Destroy(gameObject);
    }
}
