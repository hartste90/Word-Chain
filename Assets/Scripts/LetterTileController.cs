using System;
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

    private Animation anim;
    private bool isUsed = false;

    public UnityAction<LetterTileController> pressedCallback;

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
    }

    public void SetTileBackgroundColor(Color color)
    {
        // background.color = color;
    }

    public void RevertUsedTile()
    {
        PlayReadyAnimation();
        
        usedOverlay.SetActive(false);
        isUsed = false;
        // background.color = availableColor;
    }

    public void SetTileUsed()
    {
        // background.color = usedColor;
        PlayUsedAnimation();
        wrapper.DOPunchScale(Vector3.one * .5f, .1f);
        // usedOverlay.SetActive(true);
        isUsed = true;
    }

    public void OnPressed()
    {
#if UNITY_EDITOR
        pressedCallback(this);
#endif
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            pressedCallback(this);
        }
    }

//     public void OnMouseOver()
//     {
//         if (!isUsed)
//         {
// #if UNITY_EDITOR
//             if (Input.GetMouseButton(0))
//             {
//                 pressedCallback(this);
//             } 
// #else
//             // if (Input.touchCount == 1)
//             // {
//                 pressedCallback(this);
//             // }
// #endif
//         }
//     }

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
        isUsed = false;
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
        isUsed = false;
    }

    private void PlayEnterAnimation()
    {
        StartCoroutine(PlayTileEnterImpl());
    }

    IEnumerator PlayTileEnterImpl()
    {
        float delay = UnityEngine.Random.Range(0, .2f);
        yield return new WaitForSeconds(delay);
        anim.Play("TileEnter");
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

    // public void OnTileExitAnimationComplete()
    // {

    // }
}
