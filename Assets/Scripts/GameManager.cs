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


    void InitGame()
    {
        doingSetup = true;
        processingTurn = false;
        boardScript.SetupScene(0);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void ResolvePressures()
    {

    }

    void UpdatePressureZones()
    {

    }

    IEnumerator CalculateTurn()
    {
        for (int n = 0; n < 200; n++)
        {
            Debug.Log("calculating turn");
            Debug.Log("pressurezone count: " + boardScript.GetPressureZones().Count);

            List<PressureZone> spawnedPressureZones = new List<PressureZone>();
            Dictionary<Vector2, int> newPressureZoneData = new Dictionary<Vector2, int>();

            for (int i = 0; i < boardScript.GetPressureZones().Count; i++)
            {
                Debug.Log("i is " + i + " count is: " + boardScript.GetPressureZones().Count);
                PressureZone currentZone = boardScript.GetPressureZones()[i];
                Debug.Log("id: " + currentZone.GetId());

                // check pressure if it this zone exerts pressure
                if (currentZone.ExertedPressure != 0)
                {
                    Vector2 currentZonePosition = currentZone.transform.position;

                    // must check neighbors
                    Dictionary<string, PressureZone> neighbors = currentZone.CheckNeighbors();
                    foreach (KeyValuePair<string, PressureZone> zone in neighbors)
                    {
                        // if there is an existing pressure zone, add pressure to it
                        if (zone.Value != null)
                        {
                            zone.Value.IncrementPressure(currentZone.ExertedPressure);
                        }
                        // else add to pressure values to put into pressure creator
                        else
                        {
                            //// neighbor's position
                            Vector2 adjustedVector = Directions.directionFromCenter[zone.Key](currentZonePosition);

                            // create new pressure zone creation entry
                            if (newPressureZoneData.Keys.Count < 1 || !newPressureZoneData.ContainsKey(adjustedVector))
                            {
                                newPressureZoneData.Add(adjustedVector, currentZone.ExertedPressure);
                            }
                            // add to existing pressure zone creation entry
                            else
                            {
                                newPressureZoneData[adjustedVector] += currentZone.ExertedPressure;
                            }
                        }
                    }
                }
                else { }

            }
            yield return new WaitForSeconds(turnDelay);
        }
        //processingTurn = false;

        //ResolvePressures();
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
