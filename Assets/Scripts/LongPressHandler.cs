using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LongPressHandler : MonoBehaviour
{
    public UnityEvent onLongPress;
    public float longPressThreshold = .15f;
    private float timeDown;
    private bool pressing = false;

    public void OnMouseDown()
    {
        pressing = true;
        timeDown = Time.time;
    }

    public void OnMouseUp()
    {
        pressing = false;
    }

    public void OnMouseExit()
    {
        pressing = false;
    }

    void Update(){
        if (pressing)
        {
            if (Time.time - timeDown > longPressThreshold)
            {
                onLongPress.Invoke();
            }
        }
    }
}
