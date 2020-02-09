using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : PressureZone
{

    new public static int MaxPressure = int.MaxValue;
    new public static int MinPressure = int.MinValue;


    public override GameObject SpawnOnOverPressure() {
        return null;
    }

    public override void ZeroPressure() {
        
    }

    public override int CheckPressure() {
        return 0;
    }

    // Start is called before the first frame update
    new void Awake()
    {
        colorShade = TwoBitColor.DARKEST;
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
