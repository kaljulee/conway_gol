using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceholderGameboyColored : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    protected void Awake(string color)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ColorPlaceholder(color);
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
