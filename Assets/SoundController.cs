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
    public AudioClip coinDooberInPurseSound;
    public AudioClip usePowerupSound;
    public AudioClip backspaceSound;
    public AudioClip pressUIButtonSound;
    public AudioClip badSubmissionSound;
    public AudioClip tileExplodeSound;

    public AudioClip backgroundMusicSound;

    private bool isMuted = false;
    private float volume = 1f;


    public void OnMuteButtonPressed()
    {
        ToggleMute();
    }

    public static void Mute (bool shouldMute)
    {
        Instance.isMuted = shouldMute;
        Instance.listener.enabled = Instance.isMuted;
        GameController.UpdateMuteUI(Instance.isMuted);
        PlayerPrefs.SetInt(AnalyticsKeys.is_muted, Instance.isMuted == false ? 0 : 1);
    }

    private void Start()
    {
        isMuted = PlayerPrefs.GetInt(AnalyticsKeys.is_muted, 0) == 1 ? true : false;
        Mute(isMuted);
        Instance.musicSource.Play();
    }

    public void PlaySoundEffectImpl()
    {
        PlaySoundEffect();
    }

    public static void PlaySoundEffect()
    {
        Debug.Log("Playing sound");
        //Instance.effectSource.PlayOneShot(Instance.soundEffect);
    }

    public static void PlayTileTapSound()
    {
        Instance.effectSource.PlayOneShot(Instance.tileTapSound);
    }

    private void ToggleMute()
    {
        Mute(!isMuted);
    }
}
