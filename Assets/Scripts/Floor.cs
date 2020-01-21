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
        base.Awake(FourColor.LIGHTEST);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
