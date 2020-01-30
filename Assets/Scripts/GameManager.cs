using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using static Action;
using static Action.Factory;
public class GameManager : MonoBehaviour, IsBoardDirector, IsBoardActor {

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

    public void RequestManualSteps(int value) {
        RequestedManualSteps.AddLast(value);
    }


    //returns whether or not there are more steps
    private int TakeManualStep() {
        if (manualSteps > 0) {
            return manualSteps--;
        }
        if (manualSteps < 0) {
            return manualSteps++;
        }
        return 0;
    }

    private bool doingSetup;
    private bool processingTurn;
    private List<Vector2> newPressureZoneAddresses = new List<Vector2>();
    private LinkedList<Action> createActionsQueue = new LinkedList<Action>();
    private LinkedList<Action> removeActionsQueue = new LinkedList<Action>();
    private LinkedList<Action> changeActionsQueue = new LinkedList<Action>();
    private LinkedList<Action> zeroActionsQueue = new LinkedList<Action>();

    //////////////////////////
    /// handle spawnsites
    public void ApplyRandomSpawnSites(float frequency) {
        CreateRandomSpawnSites(frequency);
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.ResetBoardState();
    }

    private void CreateRandomSpawnSites(float frequency) {
        SpawnSites.Clear();
        foreach (Vector3 position in boardScript.GetGridPositions()) {
            if (Random.Range(0f, 1f) < frequency) {
                SpawnSites.AddFirst(position);
            }
        }
    }


    //////////////////////////////
    /// util stuff
    /// 

    private GameObject GetZoneByAddress(Vector2 address) {
        return boardScript.GetPressureZones().Find(zone => (Vector2)zone.transform.position == address);
    }

    void InitGame() {
        SpawnSites.Clear();
        // toad
        //SpawnSites.AddFirst(new Vector2(2, 3));
        //SpawnSites.AddFirst(new Vector2(3, 3));
        //SpawnSites.AddFirst(new Vector2(4, 3));

        // blinker
        //SpawnSites.AddFirst(new Vector2(3, 4));
        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(5, 4));

        // glider
        SpawnSites.AddFirst(new Vector2(3, 4));
        SpawnSites.AddFirst(new Vector2(4, 4));
        SpawnSites.AddFirst(new Vector2(3, 3));
        SpawnSites.AddFirst(new Vector2(4, 5));
        SpawnSites.AddFirst(new Vector2(2, 5));

        // reverse glider
        //SpawnSites.AddFirst(new Vector2(3, 4));
        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(4, 3));
        //SpawnSites.AddFirst(new Vector2(3, 5));
        //SpawnSites.AddFirst(new Vector2(5, 5));


        //SpawnSites.AddFirst(new Vector2(4, 4));
        //SpawnSites.AddFirst(new Vector2(3, 5));



        doingSetup = true;
        processingTurn = false;
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.SetupScene(0);
    }

    public void ResetGameState() {
        //Paused = true;
        boardScript.ResetBoardState();
        ActionController.instance.ClearHistory();
        //Paused = false;
    }

    public static void TogglePaused() {
        Paused = !Paused;
    }

    public static void SetPaused() {
        Paused = true;
    }

    public static void SetUnPaused() {
        Paused = false;
    }

    ///////////////////////
    /// individual methods
    /// check for reversible action calls

    protected void RemovePressureZone(GameObject zone) {
        boardScript.RemovePressureZone(zone);
        Destroy(zone);
    }

    protected void AddPressureZone(GameObject zone) {
        boardScript.AddPressureZone(zone);
        // if zone is not on the board
        if (!boardScript.ZoneIsOnBoard(zone)) {
            IssueAction(CreateDirectAction(ActionTypes.REMOVE, zone.GetComponent<PressureZone>().Pressure, zone));
        }
    }

    protected void InstantiatePressureZone(Vector2 address, float payload) {

        GameObject instance;
        if (payload > PressureZone.MaxPressure) {
            // spawn nothing if overpressured for unit as well
            if (payload > Unit.MaxPressure) {
                return;
            }
            instance = Instantiate(unitTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        else {
            instance = Instantiate(pressureZoneTile, new Vector2(), Quaternion.identity) as GameObject;
        }
        instance.transform.position = address;
        instance.GetComponent<PressureZone>().IncrementPressure((int)payload);
        AddPressureZone(instance);

    }

    public void ResolvePressureZone(GameObject zone) {

        PressureZone pressureScript = zone.GetComponent<PressureZone>();

        int pressureResult = pressureScript.CheckPressure();
        GameObject replacement = null;
        switch (pressureResult) {
            case 1:
                replacement = pressureScript.SpawnOnOverPressure();
                break;
            case -1:
                replacement = pressureScript.SpawnOnUnderPressure();
                break;
            default:
                break;
        }
        if (pressureResult != 0) {
            Vector2 position = zone.transform.position;
            removeActionsQueue.AddLast(CreateDirectAction(ActionTypes.REMOVE, zone.GetComponent<PressureZone>().Pressure, zone));
            if (replacement != null) {
                // create on replacement
                GameObject instance = Instantiate(replacement, position, Quaternion.identity) as GameObject;
                createActionsQueue.AddLast(CreateDirectAction(ActionTypes.CREATE, pressureScript.Pressure, instance));

            }
        }
        //else {
            zeroActionsQueue.AddLast(CreateDirectAction(ActionTypes.PRESSURE_ZERO, pressureScript.Pressure, zone));
        //}
    }

    private void PressureExistingNeighbor(GameObject neighbor, int exertedPressure) {
        if (neighbor == null) {
            return;
        }
        PressureZone neighborScript = neighbor.GetComponent<PressureZone>();
        neighborScript.IncrementPressure(exertedPressure);
    }
    private void SchedulePressureNeighbor(KeyValuePair<string, GameObject> neighborPair, GameObject currentZone) {
        PressureZone pressureScript = currentZone.GetComponent<PressureZone>();
        Vector2 currentZonePosition = currentZone.transform.position;

        // if there is an existing pressure zone, add pressure to it
        if (neighborPair.Value != null) {
            changeActionsQueue.AddLast(CreateDirectAction(ActionTypes.PRESSURE_CHANGE, pressureScript.ExertedPressure, neighborPair.Value));
        }
        // else add to pressure values to put into pressure creator
        else {
            // neighbor's position
            Vector2 adjustedVector = Directions.directionFromCenter[neighborPair.Key](currentZonePosition);

            if (!newPressureZoneAddresses.Contains(adjustedVector)) {
                createActionsQueue.AddLast(CreateAddressAction(ActionTypes.CREATE, 0, adjustedVector));
                newPressureZoneAddresses.Add(adjustedVector);
            }

            changeActionsQueue.AddLast(CreateAddressAction(ActionTypes.PRESSURE_CHANGE, pressureScript.ExertedPressure, adjustedVector));
        }
    }


    ////////////////////////////////
    /// foreachers
    /// 

    private void IssueActionList(LinkedList<Action> list) {
        if (list.Count == 0) {
            return;
        }
        LinkedListNode<Action> node = list.First;
        while (node != null) {
            IssueAction(node.Value);
            node = node.Next;
        }
        list.Clear();
    }
    private void IssueCreateActions() {
        IssueActionList(createActionsQueue);
        newPressureZoneAddresses.Clear();
    }
    private void IssuePressureChangeActions() {
        IssueActionList(changeActionsQueue);
    }

    private void IssueRemoveActions() {
        IssueActionList(removeActionsQueue);
    }

    private void IssueZeroActions() {
        IssueActionList(zeroActionsQueue);
    }

    void UpdatePressureZones() {
        List<GameObject> ExistingPressureZones = boardScript.GetPressureZones();
        for (int i = 0; i < ExistingPressureZones.Count; i++) {
            GameObject currentZone = ExistingPressureZones[i];
            PressureZone pressureScript = currentZone.GetComponent<PressureZone>();
            // check pressure if it this zone exerts pressure
            if (pressureScript.ExertedPressure != 0) {
                // must check neighbors
                Dictionary<string, GameObject> neighbors = pressureScript.CheckNeighbors();

                //PrintNeighborDebug(pressureScript);
                SchedulePressureNeighbors(neighbors, currentZone);
            }
            else { 
            }

        }
    }

    private void SchedulePressureNeighbors(Dictionary<string, GameObject> neighbors, GameObject currentZone) {

        foreach (KeyValuePair<string, GameObject> neighborPair in neighbors) {
            SchedulePressureNeighbor(neighborPair, currentZone);
        }

    }

    void ResolvePressureZones() {

        foreach (GameObject zone in boardScript.GetPressureZones()) {
            ResolvePressureZone(zone);
        }
    }


    /////////////////////////
    // run turn
    IEnumerator CalculateTurn() {
        for (int n = 0; n > -1; n++) {
            int step = TakeManualStep();
            if ((n < 40 && n != 0 && !Paused) || (step > 0)) {
                ActionController.instance.BeginNewRound();
                UpdatePressureZones();

                IssueCreateActions();
                IssuePressureChangeActions();

                ResolvePressureZones();
                IssueRemoveActions();
                IssueCreateActions();
                IssueZeroActions();

            }
            else if (step < 0) {
                ActionController.instance.Rewind();
            }

            yield return new WaitForSeconds(turnDelay);
        }
        //processingTurn = false;
    }


    ////////////////////////////////////
    /// action creators
    /// 

    public Action IssueAction(Action action) {
        ActionController.instance.ExecuteAction(action);
        return action;
    }

    public Action IssueAddressBoardDirection(int actionType, float payload, Vector2 address) {
        Action addressAction = CreateAddressAction(actionType, payload, address);
        return IssueAction(addressAction);
    }

    public Action IssueDirectBoardDirection(int actionType, float payload, GameObject target) {
        Action directAction = CreateDirectAction(actionType, payload, target);
        return IssueAction(directAction);
    }

    public void ExecuteBoardAction(Action action) {
        switch (action.ActionType) {
            case ActionTypes.CREATE:
                if (action.Target) {
                    AddPressureZone(action.Target);
                }
                else {
                    InstantiatePressureZone((Vector2)action.Address, 0);
                }
                break;
            case ActionTypes.PRESSURE_CHANGE:
                if (action.Target != null) {
                    PressureExistingNeighbor(action.Target, (int)Mathf.Round(action.Payload));
                }
                else {
                    PressureExistingNeighbor(GetZoneByAddress((Vector2)action.Address), (int)Mathf.Round(action.Payload));
                }
                break;
            case ActionTypes.PRESSURE_ZERO:
                if (action.Target) {
                    action.Target.GetComponent<PressureZone>().ZeroPressure();
                }
                else {
                    GetZoneByAddress((Vector2)action.Address).GetComponent<PressureZone>().ZeroPressure();
                }
                break;
            case ActionTypes.REMOVE:
                if (action.Target) {
                    RemovePressureZone(action.Target);
                }
                else {
                    RemovePressureZone(boardScript.GetPressureZones().Find(zone => zone.transform.position == action.Address));
                }
                break;
            default:
                break;
        }
    }

    //////////////////////////////////
    /// MonoBehavior methods
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    // Start is called before the first frame update
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<Camera>().backgroundColor = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHTEST);
    }

    // Update is called once per frame
    void Update() {
        if (manualSteps == 0 && RequestedManualSteps.Count > 0) {
            manualSteps = RequestedManualSteps.First.Value;
            RequestedManualSteps.RemoveFirst();
        }
        if (processingTurn) {
            return;
        }
        processingTurn = true;
        StartCoroutine(CalculateTurn());
    }


    ///////////////////////////
    /// debug
}
