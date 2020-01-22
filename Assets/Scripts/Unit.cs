using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PressureZone
{
    public float moveTime = 0.1f;

    private float inverseMoveTime;

    //protected PressureZone CheckDirection(int xDir, int yDir, out RaycastHit2D hit)
    //{
    //    Vector2 start = transform.position;
    //    Vector2 end = start + new Vector2(xDir, yDir);

    //    boxCollider.enabled = false;
    //    hit = Physics2D.Linecast(start, end, blockingLayer);
    //    boxCollider.enabled = true;

    //    if (hit.transform == null)
    //    {
    //        return null;
    //    }

    //    return hit.transform.GetComponent<PressureZone>();
    //}

    //public Dictionary<string, PressureZone> CheckNeighbors ()
    //{

    //    PressureZone up, down, left, right, upRight, upLeft, downRight, downLeft;

    //    RaycastHit2D hit;
    //    // above
    //    up = CheckDirection(0, 1, out hit);
    //    if (up)
    //    {
    //        //Debug.Log("hit above");
    //    }
    //    // right
    //    right = CheckDirection(1, 0, out hit);
    //    if (right)
    //    {
    //        //Debug.Log("hit to right");
    //    }
    //    // below
    //    down = CheckDirection(0, -1, out hit);
    //    if (down)
    //    {
    //        //Debug.Log("hit below");
    //    }
    //    // left
    //    left = CheckDirection(-1, 0, out hit);
    //    if (left)
    //    {
    //        //Debug.Log("hit to left");
    //    }
    //    upRight = CheckDirection(1, 1, out hit);
    //    upLeft = CheckDirection(-1, 1, out hit);
    //    downRight = CheckDirection(1, -1, out hit);
    //    downLeft = CheckDirection(-1, -1, out hit);
       
    //    return new Dictionary<string, PressureZone>(){ 
    //        { "up", up },
    //        {"down", down },
    //        {"left", left },
    //        { "left", left },
    //        {"right", right },
    //        {"upLeft", upLeft },
    //        {"upRight", upRight },
    //        {"downLeft", downLeft },
    //        {"downRight", downRight }
    //    };
    //}
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
