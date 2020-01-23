using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public BoardManager boardScript;
    public PressureZone pressureZoneTile;
    public Unit unitTile;
    public float turnDelay = 3.5f;

    private bool doingSetup;
    private bool processingTurn;
    private Dictionary<Vector2, int> newPressureZoneData = new Dictionary<Vector2, int>();


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
        List<PressureZone> zonesToDelete = new List<PressureZone>();
        List<PressureZone> zonesToAdd = new List<PressureZone>();
        foreach (PressureZone zone in boardScript.GetPressureZones())
        {
            //Debug.Log("zone being resolved " + zone.GetId() + " p:" + zone.Pressure);
            if (zone.Pressure > 3)
            {
                Debug.Break();
            }
            int pressureResult = zone.CheckPressure();
            PressureZone replacement = null;
            switch(pressureResult)
            {
                case 1:
                    replacement = zone.SpawnOnOverPressure();
                    break;
                case -1:
                    replacement = zone.SpawnOnUnderPressure();
                    break;
                default:
                    break;
            }
            if (pressureResult != 0)
            {
                Debug.Log("pessure change result: " + pressureResult + " p:" + zone.Pressure);
                Vector2 position = zone.transform.position;
                zonesToDelete.Add(zone);
                if (replacement != null)
                {
                    //PressureZone instance = Instantiate(replacement, position, Quaternion.identity);
                    ////Debug.Log("added zone has pressure of " + instance.Pressure);
                    //zonesToAdd.Add(instance);
                    ////boardScript.AddPressureZone(instance);
                    
                }
            } else
            {
                Debug.Log("pressure change result was zero: p" + zone.Pressure + " " + zone.CheckPressure());
                //Debug.Log("zeroing zone's pressure");
                zone.ZeroPressure();
                //Debug.Log("zone " + zone.GetId() + " p:" + zone.Pressure);
            }
        }

        // will clear out zones here
        foreach (PressureZone zone in zonesToDelete)
        {
            Debug.Log("removing zone " + zone.GetId() + " with pressure " + zone.Pressure);
            boardScript.RemovePressureZone(zone);
            Destroy(zone);
        }
        // will add zones
        //foreach (PressureZone zone in zonesToAdd)
        //{
        //    boardScript.AddPressureZone(zone);
        //}

        foreach (KeyValuePair<Vector2, int> data in newPressureZoneData)
        {
            //PressureZone instance;
            //if (data.Value > PressureZone.MaxPressure)
            //{
            //    instance = Instantiate(unitTile, new Vector2(), Quaternion.identity);
            //} else
            //{
            //    instance = Instantiate(pressureZoneTile, new Vector2(), Quaternion.identity);
            //}
            //instance.transform.position = data.Key;
            //boardScript.AddPressureZone(instance);

        }
    }

    void UpdatePressureZones()
    {
        for (int i = 0; i < boardScript.GetPressureZones().Count; i++)
        {
            //Debug.Log("i is " + i + " count is: " + boardScript.GetPressureZones().Count);
            PressureZone currentZone = boardScript.GetPressureZones()[i];
            //Debug.Log("id: " + currentZone.GetId());

            // check pressure if it this zone exerts pressure
            if (currentZone.ExertedPressure != 0)
            {
                Vector2 currentZonePosition = currentZone.transform.position;

                // must check neighbors
                Dictionary<string, PressureZone> neighbors = currentZone.CheckNeighbors();
                Debug.Log("neighbors dictionary for current zone " + currentZone.GetId());
                foreach (KeyValuePair<string, PressureZone> neighbor in neighbors)
                {
                    Debug.Log("neighbor " + neighbor.Key + " is real? " +  (neighbor.Value != null));
                    if (neighbor.Value != null)
                    {
                        Debug.Log("trying to print position, type is Unit? ");
                        Debug.Log((neighbor.Value.GetType() == typeof(Unit)));
                        Debug.Log((neighbor.Value.GetType() == typeof(PressureZone)));
                        Debug.Log(neighbor.Value);
                        Debug.Log("id: " + neighbor.Value.GetId() + " position: x" + neighbor.Value.transform.position.x + "y" + neighbor.Value.transform.position.y);
                    } else
                    {
                        Debug.Log("judged to be null:");
                        Debug.Log(neighbor.Value);
                    }

                }
                foreach (KeyValuePair<string, PressureZone> zone in neighbors)
                {
                    // if there is an existing pressure zone, add pressure to it
                    if (zone.Value != null)
                    {
                        zone.Value.IncrementPressure(currentZone.ExertedPressure);
                        Debug.Log("ZONE FOUND TO INC PRESSURE " + zone.Value.GetId() + " should have been inc'd by " + currentZone.ExertedPressure + " p:" + zone.Value.Pressure + " by zone " + currentZone.GetId());
                        Debug.Log("pressuring is Unit? " + (zone.Value.GetType() == typeof(Unit)));
                        Debug.Log("should have reported unit type");
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
                            //Debug.Log("adding to existing newPresZoneData x" + adjustedVector.x + "y" + adjustedVector.y);
                           
                            newPressureZoneData[adjustedVector] += currentZone.ExertedPressure;
                            //Debug.Log("ccc to be created ccc");
                            //foreach (KeyValuePair<Vector2, int> datum in newPressureZoneData)
                            //{
                            //    Debug.Log("x" + datum.Key.x + "y" + datum.Key.y + " p:" + datum.Value);
                            //}
                            //Debug.Log("ccccccccccccccccccccc");
                        }
                    }
                }
            }
            else { Debug.Log("exert no pressure"); }

        }
    }

    private void PrintPressureDebug()
    {
        Debug.Log("//////////// start of pressure data");
        foreach (KeyValuePair<Vector2, int> pair in newPressureZoneData)
        {
            Vector2 key = pair.Key;
            int value = pair.Value;
            Debug.Log("xy: " + key.x + " " + key.y + " weight: " + value);
        }
        Debug.Log("end of pressure data \\\\\\\\\\\\\\\\\\");

        Debug.Log("----- start of existing zone data");
        foreach (PressureZone zone in boardScript.GetPressureZones())
        {
            Debug.Log("existing xy: " + zone.transform.position.x + " " + zone.transform.position.y + " weight: " + zone.Pressure);
        }
        Debug.Log("end of existing zone data +++++++++++++");
    }
    IEnumerator CalculateTurn()
    {
        for (int n = 0; n < 200; n++)
        {
            //Debug.Log("calculating turn");
            //Debug.Log("pressurezone count: " + boardScript.GetPressureZones().Count);

            List<PressureZone> spawnedPressureZones = new List<PressureZone>();

            UpdatePressureZones();

            ResolvePressures();

            //PrintPressureDebug();

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
