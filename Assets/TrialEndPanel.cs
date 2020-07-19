
using UnityEngine;
using UnityEngine.Events;
public class TrialEndPanel : MonoBehaviour
{
    public TrialEndPanelAnimationCtrl anim;

    public GameController gameController;

    public UnityEvent nextLevelButtonCallback;


    public void SetTrialEndCallback(UnityAction nextTrialCallbackSet)
    {
        nextLevelButtonCallback.AddListener(nextTrialCallbackSet);
    }

    public void Show()
    {
        anim.PlayTrialCompleteEnterAnimation();
        gameController.EnableInput();
    }

    public void OnNextLevelButtonPressed()
    {
        anim.HidePanel();
        nextLevelButtonCallback?.Invoke();
        
    }
}
