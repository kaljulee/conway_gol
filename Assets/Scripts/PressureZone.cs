using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PressureZone : PlaceholderGameboyColored
{
    public static int StaticID = 1;
    public int Pressure { get; protected set; }
    public GameObject unitTile;
    public int ExertedPressure = 0;
    public static int MaxPressure = 2;
    public static int MinPressure = 1;
    private int id;

    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    public int GetId() => id;

    public void ZeroPressure() { Pressure = 0; }
    public virtual int CheckPressure()
    {
        if (Pressure < MinPressure) return -1;
        if (Pressure > MaxPressure) return 1;
        return 0;
    }

    // default pressure zones do nothing when they 'pop'
    // behavior can be applied later, but it doesn't seem
    // right to define these to go to Unit here
    public virtual GameObject SpawnOnOverPressure()
    {
        //Debug.Break();
        if (Pressure <= Unit.MaxPressure)
        {
            return unitTile;
        }
        return null;
    }

    public virtual GameObject SpawnOnUnderPressure()
    {
        return null;
    }

    public virtual int ExertPressure()
    {
        return ExertedPressure;
    }

    public virtual int IncrementPressure(int value = 1)
    {
        Pressure += value;
        return Pressure;
    }

    public virtual int DecrementPressure(int value = 1)
    {
        Pressure -= value;
        return Pressure;
    }

    protected GameObject CheckDirection(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            //Debug.Log("NO HIT");
            return null;
        }
        //Debug.Log("YES HIT");
        return hit.transform.gameObject;//GetComponent<GameObject>();
    }
    public Dictionary<string, GameObject> CheckNeighbors()
    {
        GameObject up, down, left, right, upRight, upLeft, downRight, downLeft;

        RaycastHit2D hit;

        // above
        up = CheckDirection(0, 1, out hit);
        if (up != null)
        {
            //Debug.Log("hit above");
            //Debug.Log(hit.transform.gameObject);
        }
        // right
        right = CheckDirection(1, 0, out hit);
        if (right != null)
        {
            //Debug.Log("hit to right");
            //Debug.Log(hit.transform.gameObject);
        }
        // below
        down = CheckDirection(0, -1, out hit);
        if (down != null)
        {
            //Debug.Log("hit below");
            //Debug.Log(hit.transform.gameObject);
        }
        // left
        left = CheckDirection(-1, 0, out hit);
        if (left != null)
        {
            //Debug.Log("hit to left");
            //Debug.Log(hit.transform.gameObject);
        }
        upRight = CheckDirection(1, 1, out hit);
        if (upRight != null)
        {
            //Debug.Log("upRight hit");
            //Debug.Log(hit.transform.gameObject);
        }
        upLeft = CheckDirection(-1, 1, out hit);
        if (upLeft != null)
        {
            //Debug.Log("upLeft hit");
            //Debug.Log(hit.transform.gameObject);
        }
        downRight = CheckDirection(1, -1, out hit);
        if (downRight != null)
        {
            //Debug.Log("downRight hit");
            //Debug.Log(hit.transform.gameObject);
        }
        downLeft = CheckDirection(-1, -1, out hit);
        if (downLeft != null)
        {
            //Debug.Log("downLeft hit");
            //Debug.Log(hit.transform.gameObject);
        }

        return new Dictionary<string, GameObject>(){
            { "up", up },
            {"down", down },
            {"left", left },
            {"right", right },
            {"upLeft", upLeft },
            {"upRight", upRight },
            {"downLeft", downLeft },
            {"downRight", downRight }
        };
    }

    new protected void Awake()
    {
        this.id = Interlocked.Increment(ref StaticID);
        if (colorShade == null)
        {
            colorShade = TwoBitColor.LIGHT;
        }
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
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
