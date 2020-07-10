﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class LetterTileController : MonoBehaviour
{
    public Transform wrapper;
    public TextMeshProUGUI letterText;
    public Image background;
    public Color availableColor;
    public Color usedColor;
    public int diceIdx;

    private Animation anim;

    public UnityAction<LetterTileController> pressedCallback;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }
    
    public void SetTileText(string textSet)
    {
        letterText.text = textSet;
    }

    public void SetTileBackgroundColor(Color color)
    {
        // background.color = color;
    }

    public void SetTileAvailable()
    {
        // background.color = availableColor;
        // PlayReadyAnimation();
    }

    public void RevertUsedTile()
    {
        PlayReadyAnimation();
        // background.color = availableColor;
    }

    public void SetTileUsed()
    {
        // background.color = usedColor;
        PlayUsedAnimation();
    }

    public void OnPressed()
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
        anim.Play("TileIncorrect");
    }

    public void PlayTileEnter()
    {
        wrapper.position = new Vector3(wrapper.position.x, 5000f, wrapper.position.z);
        StartCoroutine(PlayTileEnterImpl());
        
    }

    IEnumerator PlayTileEnterImpl()
    {
        float delay = UnityEngine.Random.Range(0, .2f);
        yield return new WaitForSeconds(delay);
        anim.Play("TileEnter");
    }
}
