using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialEndPanel : MonoBehaviour
{
    TrialEndPanelAnimationCtrl anim;
    // [SerializeField] Animation anim;
    void Start()
    {
        anim = GetComponent<TrialEndPanelAnimationCtrl>();
        anim.PlayTrialCompleteEnterAnimation();
    }


    public void OnNextLevelButtonPressed()
    {
        anim.PlayTrialCompleteEnterAnimation();
    }

    // void PlayTrialCompleteEnterAnimation()
    // {
    //     anim.Play("TrialCompleteEnter");
    // }
}
