using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floor : PlaceholderGameboyColored
{
    // Start is called before the first frame update

    new void Awake()
    {
        colorShade = TwoBitColor.LIGHTEST;
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
