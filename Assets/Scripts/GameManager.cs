using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public float turnDelaty = 0.1f;
    public BoardManager boardScript;

    private bool doingSetup;
    private List<Unit> units;


    void InitGame()
    {
        doingSetup = true;
        //units.Clear();
        boardScript.SetupScene(0);
    }

    //IEnumerator MoveUnits()
    //{
    //    unitsMoving = true;
    //    yield return new WaitForSeconds(turnDelay);
    //    if (units.Count == 0)
    //    {
    //        yield return new WaitForSeconds(turnDelay);
    //    }
    //}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //units = new List<Unit>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
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
