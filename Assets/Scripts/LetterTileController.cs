﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
public class LetterTileController : MonoBehaviour
{
    public Transform wrapper;
    public TextMeshProUGUI letterText;
    public Image background;
    public GameObject usedOverlay;
    public Color availableColor;
    public Color usedColor;
    public int diceIdx;
    public Image coinImage;

    private Animation anim;

    public UnityAction<LetterTileController> pressedCallback;
    public Action enterCallback;

    public bool hasCoinValue = false;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private void Randomize()
    {
        // wrapper.transform.localScale = Vector3.one * UnityEngine.Random.Range(.6f, 1f);
        // wrapper.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-45, 0), Vector3.forward);
    }
    
    public void SetTileText(string textSet)
    {
        letterText.text = textSet;
        hasCoinValue = MoneyController.ShouldBeCoinLetter(textSet);
        coinImage.gameObject.SetActive(hasCoinValue);

    }

    public void SetTileBackgroundColor(Color color)
    {
    }

    public void RevertUsedTile()
    {
        PlayReadyAnimation();
        
        usedOverlay.SetActive(false);
    }

    public void SetTileUsed()
    {
        PlayUsedAnimation();
        wrapper.DOPunchScale(Vector3.one * .5f, .1f);
    }

    public void OnPressed()
    {
#if UNITY_EDITOR
        EvokeCallback();
#endif
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            EvokeCallback();
        }
    }

    private void EvokeCallback()
    {
        pressedCallback(this);
    }

    //animations
    public void PlayReadyAnimation()
    {
        anim.Play("TileReady");
    }

    public void PlayUsedAnimation()
    {
        anim.Play("TileUsed");
    }

    public void PlayIncorrectAnimation()
    {
        usedOverlay.SetActive(false);
        anim.Play("TileIncorrect");
    }

    public void TileEnter()
    {
        usedOverlay.SetActive(false);
        wrapper.position = new Vector3(wrapper.position.x, 5000f, wrapper.position.z);
        Randomize();
        PlayEnterAnimation();
        
    }

    public void TileExit()
    {
        PlayExitAnimation();
    }

    private void PlayEnterAnimation()
    {
        StartCoroutine(PlayTileEnterImpl());
    }

    public void SetEnterCallback(Action enterCallbackSet)
    {
        enterCallback = enterCallbackSet;
    }

    IEnumerator PlayTileEnterImpl()
    {
        float delay = UnityEngine.Random.Range(0, .2f);
        yield return new WaitForSeconds(delay);
        anim.Play("TileEnter");
    }

    public void HandleTileEnterAnimationComplete()
    {
        enterCallback?.Invoke();
    }

    public Vector2 GetCoinPosition()
    {
        return coinImage.transform.position;
    }

    private void PlayExitAnimation()
    {
        StartCoroutine(PlayTileExitImpl());
    }

    IEnumerator PlayTileExitImpl()
    {
        float delay = UnityEngine.Random.Range(0, .25f);
        yield return new WaitForSeconds(delay);
        anim.Play("TileExit");
    }
}
