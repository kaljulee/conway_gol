using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PressureZone : PlaceholderGameboyColored
{
    public static int StaticID = 1;
    protected int Pressure { get; set; }
    public int ExertedPressure = 0;
    public int MaxPressure = 2;
    public int MinPressure = 1;
    private int id;

    public int GetId() => id;

    public virtual int CheckPressure()
    {
        if (Pressure < MinPressure) return -1;
        if (Pressure > MaxPressure) return 1;
        return 0;
    }

    public virtual int ExertPressure()
    {
        return ExertedPressure;
    }

    public virtual int IncrementPressure(int value=1)
    {
        Pressure += value;
        return Pressure;
    }

    public virtual int DecrementPressure(int value=1)
    {
        Pressure -= value;
        return Pressure;
    }


    new protected void Awake()
    {
        this.id = Interlocked.Increment(ref StaticID);
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
