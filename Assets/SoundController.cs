using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    #region singleton
    private static SoundController instance;
    public static SoundController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SoundController>();
                if (instance == null)
                {
                    Debug.LogError("No SoundController was found in this scene. Make sure you place one in the scene.");
                    return instance;
                }
            }
            return instance;
        }
    }
    #endregion

    public AudioListener listener;
    public AudioSource effectSource;
    public AudioSource musicSource;

    public AudioClip tileTapSound;
    public AudioClip completeWordSound;
    public AudioClip completeQuestSound;
    public AudioClip completeLevelSound;
    public AudioClip levelCompleteScreenSound;
    public AudioClip awardCoinsSound;
    public AudioClip coinDooberInPurseSound;
    public AudioClip usePowerupSound;
    public AudioClip backspaceSound;
    public AudioClip pressUIButtonSound;
    public AudioClip badSubmissionSound;
    public AudioClip tileExplodeSound;

    private bool isMuted = false;
    private float volume = 1f;


    public void OnMuteButtonPressed()
    {
        ToggleMute();
    }

    public static void Mute (bool shouldMute)
    {
        Instance.isMuted = shouldMute;
        Instance.musicSource.mute = Instance.isMuted;
        Instance.effectSource.mute = Instance.isMuted;
        GameController.UpdateMuteUI(Instance.isMuted);
        int mutedSet = Instance.isMuted == false ? 0 : 1;
        PlayerPrefs.SetInt(AnalyticsKeys.is_muted, mutedSet);
    }

    private void Start()
    {
        int mutedSet = PlayerPrefs.GetInt(AnalyticsKeys.is_muted, 0);
        isMuted = mutedSet == 1 ? true : false;
        Mute(isMuted);
        Instance.musicSource.Play();
    }

    public static void PlayTileTap()
    {
        Instance.effectSource.PlayOneShot(Instance.tileTapSound);
    }

    public static void PlayCompleteWord()
    {
        Instance.effectSource.PlayOneShot(Instance.completeWordSound);
    }
    public static void PlayCompleteQuest()
    {
        Instance.effectSource.PlayOneShot(Instance.completeQuestSound);
    }

    public static void PlayCompleteLevel()
    {
        Instance.effectSource.PlayOneShot(Instance.completeLevelSound);
    }

    public static void PlayAwardCoins()
    {
        Instance.effectSource.PlayOneShot(Instance.awardCoinsSound);
    }

    public static void PlayCoinToPurse()
    {
        Instance.effectSource.PlayOneShot(Instance.coinDooberInPurseSound);
    }

    public static void PlayPowerup()
    {
        Instance.effectSource.PlayOneShot(Instance.usePowerupSound);
    }

    public static void PlayBackspace()
    {
        Instance.effectSource.PlayOneShot(Instance.backspaceSound);
    }
    public static void PlayUITap()
    {
        Instance.effectSource.PlayOneShot(Instance.pressUIButtonSound);
    }

    public static void PlayBadSubmission()
    {
        Instance.effectSource.PlayOneShot(Instance.badSubmissionSound);
    }

    public static void PlayTileExplode()
    {
        Instance.effectSource.PlayOneShot(Instance.tileExplodeSound);
    }

    public static void PlayLevelCompleteScreen()
    {
        Instance.effectSource.PlayOneShot(Instance.levelCompleteScreenSound);
    }


    private void ToggleMute()
    {
        Mute(!isMuted);
    }
}
