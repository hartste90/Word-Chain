using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BackgroundBlock : MonoBehaviour
{
    public TextMeshPro letterText;
    
    private Rigidbody2D rb;
    private float recentVelocity;
    private float velocityCaptureRate = 1f;
    private float lastCaptureTime;
    public void Initialize(string letter)
    {
        letterText.text = letter;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (0f, -2f);
        CaptureVelocity();
    }

    void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (Time.time - lastCaptureTime > velocityCaptureRate)
            {
                CaptureVelocity();
            }
        }
    }

    private void CaptureVelocity()
    {
        if (recentVelocity < 1f && rb.velocity.magnitude < 1f)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            recentVelocity = rb.velocity.magnitude;
            lastCaptureTime = Time.time;
        }
        
    }
}
