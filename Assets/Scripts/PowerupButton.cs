using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupButton : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public string playerPrefsCounterKey = "undefined";
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void PlayUnavailableAnimation()
    {
        anim.Play("PowerupUnavailable");
    }

    public void PlayAddCounterAnimation()
    {
        anim.Play("PowerupAddCounter");
    }

    public void UpdateCounter()
    {
        int count = PlayerPrefs.GetInt(playerPrefsCounterKey, 0);
        counterText.text = count.ToString();
 
    }

    public void SetCounterText(string textSet)
    {
        counterText.text = textSet;
    }
}
