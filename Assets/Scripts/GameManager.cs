using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public BoardManager boardScript;
    public GameObject pressureZoneTile;
    public GameObject unitTile;
    public float turnDelay = 1.5f;

    private bool doingSetup;
    private bool processingTurn;
    private Dictionary<Vector2, int> newPressureZoneData = new Dictionary<Vector2, int>();
    List<GameObject> zonesToDelete = new List<GameObject>();
    List<GameObject> zonesToAdd = new List<GameObject>();
    //private PressureZone pressureZone;

    void InitGame()
    {
        doingSetup = true;
        processingTurn = false;
        boardScript.SetupScene(0);
    }
    
    protected void ResolveZonesToDelete ()
    {
        foreach (GameObject zone in zonesToDelete)
        {
            RemovePressureZone(zone);
        }
        zonesToDelete.Clear();
    }
    
    protected void ResolveZonesToAdd()
    {
        foreach (GameObject zone in zonesToAdd)
        {
            AddPressureZone(zone);
        }
        zonesToAdd.Clear();
    }
    
    protected void RemovePressureZone(GameObject zone)
    {
        PressureZone pressureScript = zone.GetComponent<PressureZone>();
        Debug.Log("removing zone " + pressureScript.GetId() + " with pressure " + pressureScript.Pressure);
        boardScript.RemovePressureZone(zone);
        Destroy(zone);
    }
    
    protected void AddPressureZone(GameObject zone)
    {
        boardScript.AddPressureZone(zone);
    }
    
    protected void InstantiatePressureZone(KeyValuePair<Vector2, int> data)
    {
        ////this is probably not necessary, as gameObject does not exist yet
        ////PressureZone pressureScript = data.Value.GetComponent<PressureZone>();
        
        GameObject instance;
        if (data.Value > PressureZone.MaxPressure)
        {
            // spawn nothing if overpressured for unit as well
            if (data.Value > Unit.MaxPressure)
            {
                return;
            }
            instance = Instantiate(unitTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        else
        {
            instance = Instantiate(pressureZoneTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        instance.transform.position = data.Key;
        AddPressureZone(instance);
    }
    
    public void InstantiateNewPressureZones()
    {
        foreach (KeyValuePair<Vector2, int> data in newPressureZoneData)
        {
            InstantiatePressureZone(data);
        }
        newPressureZoneData.Clear();
    }
    
    public void ResolvePressureZone(GameObject zone)
    {
        
            PressureZone pressureScript = zone.GetComponent<PressureZone>();
            //Debug.Log("zone being resolved " + zone.GetId() + " p:" + zone.Pressure);
            //if (pressureScript.Pressure > 3)
            //{
            //    Debug.Break();
            //}
            int pressureResult = pressureScript.CheckPressure();
            GameObject replacement = null;
            switch (pressureResult)
            {
                case 1:
                    replacement = pressureScript.SpawnOnOverPressure();
                    break;
                case -1:
                    replacement = pressureScript.SpawnOnUnderPressure();
                    break;
                default:
                    break;
            }
            if (pressureResult != 0)
            {
                if (zone.transform.position.x == 5 && zone.transform.position.y == 4)
            {
                Debug.Log("ppp x5y4 having pressure resolved as existing GameObject ppp");
                Debug.Log("pessure change result: " + pressureResult + " p:" + pressureScript.Pressure);
            }
                Vector2 position = zone.transform.position;
                zonesToDelete.Add(zone);
                if (replacement != null)
                {
                GameObject instance = Instantiate(replacement, position, Quaternion.identity) as GameObject;
                ////Debug.Log("added zone has pressure of " + instance.Pressure);
                zonesToAdd.Add(instance);

            }
        }
            else
            {
                Debug.Log("pressure change result was zero: p" + pressureScript.Pressure + " " + pressureScript.CheckPressure());
                //Debug.Log("zeroing zone's pressure");
                pressureScript.ZeroPressure();
                //Debug.Log("zone " + zone.GetId() + " p:" + zone.Pressure);
            }
    }
    
    void ResolvePressureZones()
    {

        foreach (GameObject zone in boardScript.GetPressureZones())
        {
            ResolvePressureZone(zone);                      
        }

        // will clear out zones here
        ResolveZonesToDelete();

        // will add zones
        ResolveZonesToAdd();

        InstantiateNewPressureZones();
    }
    
    private void PressureNeighbor(KeyValuePair<string, GameObject> neighborPair, GameObject currentZone)
    {
        PressureZone pressureScript = currentZone.GetComponent<PressureZone>();
        Vector2 currentZonePosition = currentZone.transform.position;

        // if there is an existing pressure zone, add pressure to it
        if (neighborPair.Value != null)
        {
            PressureZone neighborScript = neighborPair.Value.GetComponent<PressureZone>();
            neighborScript.IncrementPressure(pressureScript.ExertedPressure);
            Debug.Log("ZONE FOUND TO INC PRESSURE " + neighborScript.GetId()  + "x" + neighborPair.Value.transform.position.x + "y" + neighborPair.Value.transform.position.y + " should have been inc'd by " + pressureScript.ExertedPressure + " p:" + neighborScript.Pressure + " by zone " + pressureScript.GetId());
        
        }
        // else add to pressure values to put into pressure creator
        else
        {
            //// neighbor's position
            Vector2 adjustedVector = Directions.directionFromCenter[neighborPair.Key](currentZonePosition);

            // create new pressure zone creation entry
            if (newPressureZoneData.Keys.Count < 1 || !newPressureZoneData.ContainsKey(adjustedVector))
            {
                newPressureZoneData.Add(adjustedVector, pressureScript.ExertedPressure);
            }
            // add to existing pressure zone creation entry
            else
            {
                //Debug.Log("adding to existing newPresZoneData x" + adjustedVector.x + "y" + adjustedVector.y);

                newPressureZoneData[adjustedVector] += pressureScript.ExertedPressure;
                //Debug.Log("ccc to be created ccc");
                //foreach (KeyValuePair<Vector2, int> datum in newPressureZoneData)
                //{
                //    Debug.Log("x" + datum.Key.x + "y" + datum.Key.y + " p:" + datum.Value);
                //}
                //Debug.Log("ccccccccccccccccccccc");
            }
        }
    }
    
    private void PressureNeighbors(Dictionary<string, GameObject> neighbors, GameObject currentZone)
    {

        foreach (KeyValuePair<string, GameObject> neighborPair in neighbors)
        {
            PressureNeighbor(neighborPair, currentZone);
        }

    }
    
    private void PrintNeighborDebug(PressureZone pressureScript)
    {
        Dictionary<string, GameObject> neighbors = pressureScript.CheckNeighbors();

        //Debug.Log("neighbors dictionary for current zone " + pressureScript.GetId());
        foreach (KeyValuePair<string, GameObject> neighbor in neighbors)
        {
            //Debug.Log("neighbor " + neighbor.Key + " is real? " + (neighbor.Value != null));

            //if (neighbor.Value != null)
            //{
            //    //Debug.Log("trying to print position, type is Unit? ");
            //    //Debug.Log((neighbor.Value.GetType() == typeof(Unit)));
            //    //Debug.Log((neighbor.Value.GetType() == typeof(PressureZone)));
            //    //Debug.Log(neighbor.Value);
            //    //Debug.Log("id: " + neighbor.Value.GetId() + " position: x" + neighbor.Value.transform.position.x + "y" + neighbor.Value.transform.position.y);
            //}
            //else
            //{
            //    //Debug.Log("judged to be null:");
            //    //Debug.Log(neighbor.Value);
            //}

        }
    }
    
    void UpdatePressureZones()
    {
        List<GameObject> ExistingPressureZones = boardScript.GetPressureZones();
        for (int i = 0; i < ExistingPressureZones.Count; i++)
        {
            //Debug.Log("i is " + i + " count is: " + boardScript.GetPressureZones().Count);
            GameObject currentZone = ExistingPressureZones[i];
            //Debug.Log("id: " + currentZone.GetId());
            PressureZone pressureScript = currentZone.GetComponent<PressureZone>();

            // check pressure if it this zone exerts pressure
            if (pressureScript.ExertedPressure != 0)
            {
                //Vector2 currentZonePosition = currentZone.transform.position;

                // must check neighbors
                Debug.Log("Checking neighbors for " + pressureScript.GetId());
                Dictionary<string, GameObject> neighbors = pressureScript.CheckNeighbors();

                PrintNeighborDebug(pressureScript);

                PressureNeighbors(neighbors, currentZone);
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
        foreach (GameObject zone in boardScript.GetPressureZones())
        {
            Debug.Log("existing xy: " + zone.transform.position.x + " " + zone.transform.position.y + " weight: " + zone.GetComponent<PressureZone>().Pressure);
        }
        Debug.Log("end of existing zone data +++++++++++++");
    }
    IEnumerator CalculateTurn()
    {
        for (int n = 0; n < 30; n++)
        {
            if (n != 0)
            {
                Debug.Log("calculating turn");
                Debug.Log("pressurezone count: " + boardScript.GetPressureZones().Count);

                List<PressureZone> spawnedPressureZones = new List<PressureZone>();
                //Debug.Break();
                UpdatePressureZones();
                //Debug.Break();
                ResolvePressureZones();
                //Debug.Break();
                PrintPressureDebug();
            }

            yield return new WaitForSeconds(turnDelay);
        }
        //processingTurn = false;
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
