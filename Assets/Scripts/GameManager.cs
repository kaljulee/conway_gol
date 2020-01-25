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
    public static bool Paused { get; set; } = false;

    private bool doingSetup;
    private bool processingTurn;
    private Dictionary<Vector2, int> newPressureZoneData = new Dictionary<Vector2, int>();
    List<GameObject> zonesToDelete = new List<GameObject>();
    List<GameObject> zonesToAdd = new List<GameObject>();

    void InitGame()
    {
        doingSetup = true;
        processingTurn = false;
        boardScript.SetupScene(0);
    }
    
    public static void TogglePaused()
    {
        Paused = !Paused;
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
                Vector2 position = zone.transform.position;
                zonesToDelete.Add(zone);
                if (replacement != null)
                {
                GameObject instance = Instantiate(replacement, position, Quaternion.identity) as GameObject;
                zonesToAdd.Add(instance);

            }
        }
            else
            {
                pressureScript.ZeroPressure();
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
        
        }
        // else add to pressure values to put into pressure creator
        else
        {
            // neighbor's position
            Vector2 adjustedVector = Directions.directionFromCenter[neighborPair.Key](currentZonePosition);

            // create new pressure zone creation entry
            if (newPressureZoneData.Keys.Count < 1 || !newPressureZoneData.ContainsKey(adjustedVector))
            {
                newPressureZoneData.Add(adjustedVector, pressureScript.ExertedPressure);
            }
            // add to existing pressure zone creation entry
            else
            {
                newPressureZoneData[adjustedVector] += pressureScript.ExertedPressure;
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
            GameObject currentZone = ExistingPressureZones[i];
            PressureZone pressureScript = currentZone.GetComponent<PressureZone>();

            // check pressure if it this zone exerts pressure
            if (pressureScript.ExertedPressure != 0)
            {
                // must check neighbors
                Dictionary<string, GameObject> neighbors = pressureScript.CheckNeighbors();

                PrintNeighborDebug(pressureScript);

                PressureNeighbors(neighbors, currentZone);
            }
            else { }

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
        for (int n = 0; n > -1; n++)
        {

            if (n != 0 && !Paused)
            {

                List<PressureZone> spawnedPressureZones = new List<PressureZone>();
                UpdatePressureZones();
                ResolvePressureZones();
                //PrintPressureDebug();
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
