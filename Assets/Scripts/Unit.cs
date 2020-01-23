using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PressureZone
{
    public float moveTime = 0.1f;
    
    new public static int MaxPressure = 3;
    new public static int MinPressure = 2;
    private float inverseMoveTime;

    public override int CheckPressure()
    {
        if (Pressure > MaxPressure)
        {
            return 1;
        }
        if (Pressure < MinPressure)
        {
            return -1;
        }
        return 0;
    }
    public override GameObject SpawnOnOverPressure()
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
