using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PlaceholderGameboyColored
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    protected bool CheckDirection(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            return false;
        }

        return true;
    }

    public bool[] CheckNeighbors ()
    {

        bool above, below, left, right;

        RaycastHit2D hit;
        // above
        above = CheckDirection(0, 1, out hit);
        if (above)
        {
            Debug.Log("hit above");
        }
        // right
        right = CheckDirection(1, 0, out hit);
        if (right)
        {
            Debug.Log("hit to right");
        }
        // below
        below = CheckDirection(0, 1, out hit);
        if (below)
        {
            Debug.Log("hit below");
        }
        // left
        left = CheckDirection(1, 0, out hit);
        if (left)
        {
            Debug.Log("hit to left");
        }
        if (!above && !left && !right && !below)
        {
            Debug.Log("no hits");
        }
        //Debug.Log("tag: ");
        //Debug.Log(hit.transform.GetComponent<Floor>().tag);
       
        return new bool[4]{ false, false, false, false};
    }
    private void Awake()
    {
        base.Awake(FourColor.DARKEST);
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckNeighbors();
    }
}
