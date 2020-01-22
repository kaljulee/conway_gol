using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public BoardManager boardScript;
    public float turnDelay = 3.5f;

    private bool doingSetup;
    private bool processingTurn;
    private List<Unit> units;


    void InitGame()
    {
        doingSetup = true;
        processingTurn = false;
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

    void ApplyPressures()
    {

    }

    void UpdatePressureZones()
    {

    }

    IEnumerator CalculateTurn()
    {
        Debug.Log("calculating turn");
        for (int i = 0; i < boardScript.GetPressureZones().Count; i++)
        {
            Debug.Log("id: " + boardScript.GetPressureZones()[i].GetId());
        }
        Debug.Log("-------------done calculating-----------");
        yield return new WaitForSeconds(turnDelay);
        processingTurn = false;

        //ApplyPressures();
        //UpdatePressureZones();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (processingTurn)
        {
            return;
        }
        processingTurn = true;
        StartCoroutine(CalculateTurn());
    }
}
