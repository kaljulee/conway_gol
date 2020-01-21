using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceholderGameboyColored : MonoBehaviour
{
    protected void ColorPlaceholder (string fourColorShade)
        {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
  
        //Debug.Log(new Color(100, 10, 10));
        //Debug.Log(Color.blue);
        //spriteRenderer.color = new Color(100, 10, 10);
        spriteRenderer.color = new Color(
            FourColor.gameboyColorsRGB[fourColorShade][0],
            FourColor.gameboyColorsRGB[fourColorShade][1],
            FourColor.gameboyColorsRGB[fourColorShade][2]
            );
}
}
