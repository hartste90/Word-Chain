using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class LetterTileController : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public Image background;
    public Color availableColor;
    public Color usedColor;
    public int diceIdx;

    public UnityAction<LetterTileController> pressedCallback;

    public void SetTileText(string textSet)
    {
        letterText.text = textSet;
    }

    public void SetTileBackgroundColor(Color color)
    {
        background.color = color;
    }

    public void SetTileAvailable()
    {
        background.color = availableColor;
    }

    public void SetTileUsed()
    {
        background.color = usedColor;
    }

    public void OnPressed()
    {
        pressedCallback(this);
    }
}
