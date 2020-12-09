using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    public GameObject muteActiveImage;

    public void UpdateMuteUI (bool isMuted)
    {
        muteActiveImage.SetActive(!isMuted);
    }
}
