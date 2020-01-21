using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureZone : PlaceholderGameboyColored
{
    protected int Pressure { get; set; }
    public int ExertedPressure = 0;

   

    public int increment(int value=1)
    {
        Pressure += value;
        return Pressure;
    }

    public int decrement(int value=1)
    {
        Pressure -= value;
        return Pressure;
    }
    new protected void Awake()
    {
        Debug.Log(colorShade);
        if (colorShade == null)
        {
            colorShade = FourColor.LIGHT;
        }
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
