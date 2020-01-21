using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floor : PlaceholderGameboyColored
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;

    void Awake()
    {

        ColorPlaceholder(FourColor.LIGHTEST);

        //spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("colors in floor awakening");
        //Debug.Log(FourColor.gameboyColorsRGB);
        //Debug.Log(FourColor.LIGHTEST);
        //Debug.Log(FourColor.gameboyColorsRGB[FourColor.LIGHTEST]);
        //Debug.Log(FourColor.gameboyColorsRGB[FourColor.LIGHTEST][0]);
        ////Debug.Log(new Color(100, 10, 10));
        ////Debug.Log(Color.blue);
        ////spriteRenderer.color = new Color(100, 10, 10);
        //spriteRenderer.color = new Color(
        //    FourColor.gameboyColorsRGB[FourColor.LIGHTEST][0],
        //    FourColor.gameboyColorsRGB[FourColor.LIGHTEST][1],
        //    FourColor.gameboyColorsRGB[FourColor.LIGHTEST][2]
        //    );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
