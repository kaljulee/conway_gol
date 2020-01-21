using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceholderGameboyColored : MonoBehaviour
{
    // default color;
    public string colorShade = FourColor.LIGHTEST;
    private SpriteRenderer spriteRenderer;
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ColorPlaceholder(colorShade);
    }
    private void ColorPlaceholder (string fourColorShade)
        {
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(
            FourColor.gameboyColorsRGB[fourColorShade][0],
            FourColor.gameboyColorsRGB[fourColorShade][1],
            FourColor.gameboyColorsRGB[fourColorShade][2]
            );
}
}
