using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floor : PlaceholderGameboyColored
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;

    new void Awake()
    {
        colorShade = FourColor.LIGHTEST;
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
