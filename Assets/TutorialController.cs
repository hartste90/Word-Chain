using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<TutorialItem> tutorialStepsList;

    private int currentStep = 0;


    public void Init()
    {
        if (PlayerPrefs.GetInt(AnalyticsKeys.is_tutorial_complete, 0) == 0)
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        currentStep = 0;
        tutorialStepsList[0].Show(AdvanceTutorial);
    }

    public void AdvanceTutorial()
    {
        tutorialStepsList[currentStep].Hide();
        if (tutorialStepsList.Count > currentStep + 1)
        {
            currentStep++;
            tutorialStepsList[currentStep].Show(AdvanceTutorial);
        }
        else
        {
            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        PlayerPrefs.SetInt(AnalyticsKeys.is_tutorial_complete, 1);
    }
}

