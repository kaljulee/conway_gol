using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour, IsBoardDirector, IsBoardActor
{

    public static GameManager instance = null;
    public GameObject mainCamera;
    public BoardManager boardScript;
    public GameObject pressureZoneTile;
    public GameObject unitTile;
    public float turnDelay = 1.5f;

    private int manualSteps = 0;
    private LinkedList<int> RequestedManualSteps = new LinkedList<int>();
    public static bool Paused { get; set; } = false;

    private LinkedList<Vector2> SpawnSites = new LinkedList<Vector2>();

    public void RequestManualSteps(int value)
    {
        RequestedManualSteps.AddLast(value);
    }


    //returns whether or not there are more steps
    private bool TakeManualStep()
    {
        if (manualSteps > 0)
        {
            manualSteps -= 1;
            return true;
        }
        if (manualSteps < 0)
        {
            manualSteps += 1;
            return true;
        }
        return false;
    }

    private bool doingSetup;
    private bool processingTurn;
    private Dictionary<Vector2, int> newPressureZoneData = new Dictionary<Vector2, int>();
    List<GameObject> zonesToDelete = new List<GameObject>();
    List<GameObject> zonesToAdd = new List<GameObject>();


    //////////////////////////
    /// handle spawnsites
    public void ApplyRandomSpawnSites(float frequency)
    {
        CreateRandomSpawnSites(frequency);
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.ResetBoardState();
    }

    private void CreateRandomSpawnSites(float frequency)
    {
        SpawnSites.Clear();
        foreach (Vector3 position in boardScript.GetGridPositions())
        {
            if (Random.Range(0f, 1f) < frequency)
            {
                SpawnSites.AddFirst(position);
            }
        }
    }


    //////////////////////////////
    /// util stuff
    void InitGame()
    {
        SpawnSites.Clear();
        // toad
        //SpawnSites.AddFirst(new Vector2(2, 3));
        //SpawnSites.AddFirst(new Vector2(3, 3));
        //SpawnSites.AddFirst(new Vector2(4, 3));

        //SpawnSites.AddFirst(new Vector2(3, 4));
        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(5, 4));

        // glider
        //SpawnSites.AddFirst(new Vector2(3, 4));
        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(3, 3));
        //SpawnSites.AddFirst(new Vector2(4, 5));
        //SpawnSites.AddFirst(new Vector2(2, 5));

        // reverse glider
        SpawnSites.AddFirst(new Vector2(3, 4));
        SpawnSites.AddFirst(new Vector2(4, 4));
        SpawnSites.AddFirst(new Vector2(4, 3));
        SpawnSites.AddFirst(new Vector2(3, 5));
        SpawnSites.AddFirst(new Vector2(5, 5));


        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(3, 5));



        doingSetup = true;
        processingTurn = false;
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.SetupScene(0);
    }

    public void ResetGameState()
    {
        //Paused = true;
        boardScript.ResetBoardState();
        ActionExecutor.instance.ClearHistory();
        //Paused = false;
    }

    public static void TogglePaused()
    {
        Paused = !Paused;
    }

    public static void SetPaused()
    {
        Paused = true;
    }

    public static void SetUnPaused()
    {
        Paused = false;
    }

    ///////////////////////
    /// individual methods
    /// check for reversible action calls

    protected void RemovePressureZone(GameObject zone)
    {
        PressureZone pressureScript = zone.GetComponent<PressureZone>();
        boardScript.RemovePressureZone(zone);
        //IssueDirectBoardDirection(Action.ActionTypes.REMOVE, -1, zone);
        Destroy(zone);
    }

    protected void AddPressureZone(GameObject zone)
    {
        boardScript.AddPressureZone(zone);
        //IssueDirectBoardDirection(Action.ActionTypes.CREATE, Action.ZoneTypes.GetZoneType(zone.GetType()), zone);
    }

    protected void InstantiatePressureZone(Vector2 address, float payload)//KeyValuePair<Vector2, int> data)
    {
        ////this is probably not necessary, as gameObject does not exist yet
        ////PressureZone pressureScript = data.Value.GetComponent<PressureZone>();

        GameObject instance;
        if (payload > PressureZone.MaxPressure)
        {
            // spawn nothing if overpressured for unit as well
            if (payload > Unit.MaxPressure)
            {
                return;
            }
            instance = Instantiate(unitTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        else
        {
            instance = Instantiate(pressureZoneTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        instance.transform.position = address;
        AddPressureZone(instance);
        //IssueDirectBoardDirection(Action.ActionTypes.CREATE, Action.ZoneTypes.GetZoneType(instance.GetType()), instance);

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

    private void PressureNeighbor(KeyValuePair<string, GameObject> neighborPair, GameObject currentZone)
    {
        PressureZone pressureScript = currentZone.GetComponent<PressureZone>();
        Vector2 currentZonePosition = currentZone.transform.position;

        // if there is an existing pressure zone, add pressure to it
        if (neighborPair.Value != null)
        {
            PressureZone neighborScript = neighborPair.Value.GetComponent<PressureZone>();
            neighborScript.IncrementPressure(pressureScript.ExertedPressure);
            IssueDirectBoardDirection(Action.ActionTypes.PRESSURE_CHANGE, pressureScript.ExertedPressure, neighborPair.Value);

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


    ////////////////////////////////
    /// foreachers
    protected void ResolveZonesToDelete()
    {
        foreach (GameObject zone in zonesToDelete)
        {
            //RemovePressureZone(zone);
            IssueDirectBoardDirection(Action.ActionTypes.REMOVE, -1, zone);
        }
        zonesToDelete.Clear();
    }

    protected void ResolveZonesToAdd()
    {
        foreach (GameObject zone in zonesToAdd)
        {
            //AddPressureZone(zone);
            IssueDirectBoardDirection(Action.ActionTypes.CREATE, Action.ZoneTypes.GetZoneType(zone.GetType()), zone);
        }
        zonesToAdd.Clear();
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

                //PrintNeighborDebug(pressureScript);

                PressureNeighbors(neighbors, currentZone);
            }
            else { }

        }
    }

    private void PressureNeighbors(Dictionary<string, GameObject> neighbors, GameObject currentZone)
    {

        foreach (KeyValuePair<string, GameObject> neighborPair in neighbors)
        {
            PressureNeighbor(neighborPair, currentZone);
            // create action here
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

    public void InstantiateNewPressureZones()
    {
        foreach (KeyValuePair<Vector2, int> data in newPressureZoneData)
        {
            IssueAddressBoardDirection(Action.ActionTypes.CREATE, data.Value, data.Key);
            //InstantiatePressureZone(data);
        }
        newPressureZoneData.Clear();
    }




    /////////////////////////
    // run turn
    IEnumerator CalculateTurn()
    {
        for (int n = 0; n > -1; n++)
        {

            if ((n != 0 && !Paused) || (TakeManualStep()))
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


    ////////////////////////////////////
    /// action creators
    public Action IssueAddressBoardDirection(int actionType, float payload, Vector2 address)
    {
        Action addressAction = Action.Factory.CreateAddressAction(actionType, payload, address);
        ActionExecutor.instance.ExecuteAction(addressAction);
        return addressAction;
    }

    public Action IssueDirectBoardDirection(int actionType, float payload, GameObject target)
    {
        Action directAction = Action.Factory.CreateDirectAction(actionType, payload, target);
        ActionExecutor.instance.ExecuteAction(directAction);
        return directAction;
    }

    public void ExecuteBoardAction(Action action)
    {
        switch (action.ActionType)
        {
            case Action.ActionTypes.CREATE:
                if (action.Target)
                {
                    AddPressureZone(action.Target);
                } else
                {
                    InstantiatePressureZone((Vector2)action.Address, action.Payload);
                    Debug.LogError("no target");
                }
                break;
            case Action.ActionTypes.PRESSURE_CHANGE:

                break;
            case Action.ActionTypes.REMOVE:
                if (action.Target)
                {
                    RemovePressureZone(action.Target);
                } else
                {
                    RemovePressureZone(zonesToDelete.Find(zone => zone.transform.position == action.Address));
                }
                break;
            default:
                break;
        }
    }

    //////////////////////////////////
    /// MonoBehavior methods
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
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<Camera>().backgroundColor = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHTEST);
    }

    // Update is called once per frame
    void Update()
    {
        if (manualSteps == 0 && RequestedManualSteps.Count > 0)
        {
            manualSteps = RequestedManualSteps.First.Value;
            RequestedManualSteps.RemoveFirst();
        }
        if (processingTurn)
        {
            return;
        }
        processingTurn = true;
        StartCoroutine(CalculateTurn());
    }


    ///////////////////////////
    /// debug

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
}
