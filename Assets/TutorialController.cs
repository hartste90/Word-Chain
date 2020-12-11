using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<TutorialItem> tutorialStepsList;

    private int currentStep = 0;
    private DateTime lastAdvancedTutorialTime;

    public void Init()
    {
        if (PlayerPrefs.GetInt(AnalyticsKeys.is_tutorial_complete, 0) == 0)
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        AnalyticsController.OnTutorialStarted();
        currentStep = 0;
        tutorialStepsList[0].Show(AdvanceTutorial);
        lastAdvancedTutorialTime = DateTime.Now;
    }

    public void AdvanceTutorial()
    {
        if (HasSeenCurrentStep())
        {
            lastAdvancedTutorialTime = DateTime.Now;
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
    }

    private bool HasSeenCurrentStep()
    {
        if (DateTime.Now.CompareTo(lastAdvancedTutorialTime.AddSeconds(2)) >= 0)
        {
            return true;
        }
        return false;
    }

    private void CompleteTutorial()
    {
        AnalyticsController.OnTutorialCompleted();
        PlayerPrefs.SetInt(AnalyticsKeys.is_tutorial_complete, 1);
    }

    public bool IsTutorialComplete()
    {
        return PlayerPrefs.GetInt(AnalyticsKeys.is_tutorial_complete, 0) == 0 ? false : true;
    }
}

