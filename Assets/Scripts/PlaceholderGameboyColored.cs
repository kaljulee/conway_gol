using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceholderGameboyColored : MonoBehaviour
{
    // default color;
    public string colorShade;
    private SpriteRenderer spriteRenderer;
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (colorShade != null)
        {
            ColorPlaceholder(colorShade);
        }
    }
    protected void ColorPlaceholder(string fourColorShade)
    {
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = TwoBitColor.GenerateTwoBitColor(fourColorShade);
    }
}
