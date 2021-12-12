using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fun : MonoBehaviour
{
    public float blinkingEffect = 0.2f;

    protected float blinkingDuration = 1.0f;
    protected float lastBlinking;
    protected float immuneTime = 1.0f;
    protected float lastImmune = 0f;

    protected Vector3 pushDirection;

    private Color[] blinkingColors = new Color[] { new Color32(197, 197, 197, 255), Color.black };
    private int currentColor = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        lastImmune = Time.time;
        lastBlinking = Time.time;
    }
    private void Update()
    {
        if (Time.time - lastImmune < immuneTime && Time.time > 1)
        {
            if (Time.time - lastBlinking > blinkingEffect)
            {
                lastBlinking = Time.time;
                currentColor++;
                GetComponent<SpriteRenderer>().color = blinkingColors[currentColor % 2];
            }
        }
    }
}
