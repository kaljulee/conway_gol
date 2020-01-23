using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PressureZone
{
    public float moveTime = 0.1f;
    
    public static int maxPressure = 3;
    public static int minPressure = 2;
    private float inverseMoveTime;

    public override int CheckPressure()
    {
        if (Pressure > maxPressure)
        {
            return 1;
        }
        if (Pressure < minPressure)
        {
            return -1;
        }
        return 0;
    }
    public override PressureZone SpawnOnOverPressure()
    {
        return null;
    }
    new void Awake()
    {
        colorShade = FourColor.DARKEST;
        Pressure = 0;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
        //rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
