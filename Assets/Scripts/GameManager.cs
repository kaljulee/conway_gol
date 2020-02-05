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
    private bool stepping = false;
    public Vector2 spawnCenter = Vector2.zero;
    public bool drawMode = false;
    private bool firstTimeCameraLoad = true;

    private int manualSteps = 0;
    private LinkedList<int> RequestedManualSteps = new LinkedList<int>();
    public static bool Paused { get; set; } = false;

    private LinkedList<Vector2> SpawnSites = Templates.Blinker();//new LinkedList<Vector2>();

    public void RequestManualSteps(int value) {
        RequestedManualSteps.AddLast(value);
    }

    IEnumerator ContinuousManualSteps(int steps, float initialWait = 1.1f) {
        float wait = initialWait;
        while (stepping) {
            if (wait > 0.15) {
                wait *= 0.8f;
            }
            RequestManualSteps(steps);
            yield return new WaitForSeconds(wait);

        }
    }

    public void StartStepping(int steps) {
        stepping = true;
        StartCoroutine(ContinuousManualSteps(steps));
    }

    public void StopStepping() {
        stepping = false;
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
    private LinkedList<Vector2> drawZones = new LinkedList<Vector2>();
    private LinkedList<Vector2> shakeZones = new LinkedList<Vector2>();

    //////////////////////////
    /// handle spawnsites
    public void ApplyRandomSpawnSites(float frequency) {
        SpawnSites = CreateRandomSpawnSites(frequency);
        SetSpawnCenter(boardScript.BoardCenter());
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.ResetBoardState();
    }

    public void SetSpawnCenter(Vector2 center) => spawnCenter = center;

    public void ApplySpawnSiteTemplate(LinkedList<Vector2> template) {
        // probably remove this;
        SpawnSites.Clear();
        SetSpawnCenter(boardScript.BoardCenter());
        foreach (Vector2 site in template) {
            SpawnSites.AddFirst(site + spawnCenter);
        }
        boardScript.SetSpawnSites(SpawnSites);
        boardScript.ResetBoardState();

    }

    private void ClearQueues() {
        newPressureZoneAddresses.Clear();
        createActionsQueue.Clear();
        removeActionsQueue.Clear();
        changeActionsQueue.Clear();
        zeroActionsQueue.Clear();
        drawZones.Clear();
        shakeZones.Clear();
    }

    private void RequestZones(LinkedList<Vector2> requestedZones, LinkedList<Vector2> pendingZones) {

        foreach (Vector2 zone in requestedZones) {
            if (!pendingZones.Contains(zone)) {
                pendingZones.AddLast(zone);
            }
        }

    }

    public void RequestDrawZones(LinkedList<Vector2> zones) {
        if (drawMode) {
            RequestZones(zones, drawZones);
        }
    }

    public void RequestShakeZones(LinkedList<Vector2> zones) {
        RequestZones(zones, shakeZones);
    }

    private void ApplyZones(LinkedList<Vector2> zones) {
        foreach (Vector2 zone in zones) {
            Action delete = CreateAddressAction(ActionTypes.REMOVE, 0, zone + spawnCenter);
            Action create = CreateAddressAction(ActionTypes.CREATE, 3, zone + spawnCenter);
            //ActionController.instance.ExecuteAction(delete);
            //ActionController.instance.ExecuteAction(create);
            IssueAction(delete);
            IssueAction(create);
        }
        zones.Clear();
    }

    public void ApplyShakeZones() {
        ApplyZones(shakeZones);
    }

    public void ApplyDrawZones() {
        ApplyZones(drawZones);
    }

    public LinkedList<Vector2> CreateRandomSpawnSites(float frequency) {
        //SpawnSites.Clear();
        LinkedList<Vector2> returnValue = new LinkedList<Vector2>(); ;
        foreach (Vector3 position in boardScript.GetGridPositions()) {
            if (Random.Range(0f, 1f) < frequency) {
                returnValue.AddFirst(position);
            }
        }
        return returnValue;
    }


    //////////////////////////////
    /// util stuff
    /// 

    public void ToggleDrawMode() {
        drawMode = !drawMode;
    }

    public void ClearBoard() {
        ClearQueues();
        IssueAction(CreateAddressAction(ActionTypes.CLEAR_ALL, 0, Vector2.zero));
    }

    private GameObject GetZoneByAddress(Vector2 address) {
        return boardScript.GetPressureZones().Find(zone => (Vector2)zone.transform.position == address);
    }

    void InitGame() {
        SpawnSites.Clear();


        doingSetup = true;
        processingTurn = false;
        Vector2 boardSize = boardScript.GetBoardSize();
        Vector2 center = new Vector2((int)(boardSize.x / 2), (int)(boardSize.y / 2));
        SetSpawnCenter(center);
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
        PressureZone pressureScript = zone.GetComponent<PressureZone>();
        // if zone is not on the board
        if (!boardScript.PositionIsOnBoard(zone.transform.position)) {
            IssueAction(CreateDirectAction(ActionTypes.REMOVE, ZoneTypes.GetZoneType(pressureScript), zone));
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
            removeActionsQueue.AddLast(CreateDirectAction(ActionTypes.REMOVE, ZoneTypes.GetZoneType(pressureScript), zone));
            if (replacement != null) {
                // create on replacement
                GameObject instance = Instantiate(replacement, position, Quaternion.identity) as GameObject;
                createActionsQueue.AddLast(CreateDirectAction(ActionTypes.CREATE, pressureScript.Pressure, instance));

            }
        }
        zeroActionsQueue.AddLast(CreateDirectAction(ActionTypes.PRESSURE_ZERO, pressureScript.Pressure, zone));
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
            if (boardScript.PositionIsOnBoard(adjustedVector)) {
                if (!newPressureZoneAddresses.Contains(adjustedVector)) {
                    createActionsQueue.AddLast(CreateAddressAction(ActionTypes.CREATE, 0, adjustedVector));
                    newPressureZoneAddresses.Add(adjustedVector);
                }

                changeActionsQueue.AddLast(CreateAddressAction(ActionTypes.PRESSURE_CHANGE, pressureScript.ExertedPressure, adjustedVector));
            }
            else { }

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
                    InstantiatePressureZone((Vector2)action.Address, action.Payload);
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
            case ActionTypes.ALL_PRESSURE_ZERO:
                boardScript.GetPressureZones().ForEach(z => z.GetComponent<PressureZone>().ZeroPressure());
                break;
            case ActionTypes.SET_TEMPLATE:
                //ApplySpawnSiteTemplate(Templates.GetTemplate((int)action.Payload));

                break;
            case ActionTypes.CLEAR_ALL:
                boardScript.ClearBoard();
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
        SetSpawnCenter(boardScript.BoardCenter());
    }

    public float ConvertBoardToCameraSize(Vector2 board) {
        return board.y / 2;
    }

    public float RequiredCameraSize(Vector2 board) {
        return ConvertBoardToCameraSize(board);
    }

    public Vector3 RepositionCamera(float size, float aspect) {
        float height = size * 2;
        return new Vector3(height * aspect, height, -10);
    }


    /////////////////////////
    // run turn
    private void TakeTurn() {
        // is there a manual step forwards or backwards?
        ActionController.instance.BeginNewRound();
            int step = TakeManualStep();

         
    }

    private void TurnForwardMechanism() {
        ActionController.instance.BeginNewRound();
        UpdatePressureZones();

        IssueCreateActions();
        IssuePressureChangeActions();

        ResolvePressureZones();
        IssueRemoveActions();
        IssueCreateActions();
        IssueZeroActions();

        ActionController.instance.EndRound();
    }

    private void IncrementTime(int n, bool paused, int step) {
        if ((n != 0 && !Paused) || (step > 0)) {
            TurnForwardMechanism();

        }
        else if (step < 0) {
            TurnRewindMechanism();
        }
    }

    private void TurnRewindMechanism() {
        ActionController.instance.Rewind();
        foreach (GameObject obj in boardScript.GetPressureZones()) {
            PressureZone zone = obj.GetComponent<PressureZone>();
        }
    }
    /////////////////////////
    // run turn

    /////////////////////////
    // run turn
    IEnumerator CalculateTurn() {
        for (int n = 0; n > -1; n++) {
            // check for manual step
            int step = TakeManualStep();


            // execute forward or backward step in time
            IncrementTime(n, Paused, step);

            // apply non-calculated zones
            // draw zones
            if (drawMode && drawZones.Count > 0) {
                ActionController.instance.BeginNewRound();
                ApplyDrawZones();
            }
            // shake zones
            if (shakeZones.Count > 0) {
                ActionController.instance.BeginNewRound();
                ApplyShakeZones();
            }

            // final zero
            IssueAction(CreateAddressAction(ActionTypes.ALL_PRESSURE_ZERO, 0, Vector2.zero));

            // end of turn
            yield return new WaitForSeconds(turnDelay);
        }
        //processingTurn = false;
    }


    // Update is called once per frame
    void Update() {
        if (manualSteps == 0 && RequestedManualSteps.Count > 0) {
            manualSteps = RequestedManualSteps.First.Value;
            RequestedManualSteps.RemoveFirst();
        }

        float size = boardScript.GetBoardSize().y;
        if (Camera.main.orthographicSize != size) {
            Camera.main.orthographicSize = size / 2;
            Vector3 reposition = new Vector3((size / 2) * Camera.main.aspect, (size / 2), -10);
            Camera.main.transform.position = reposition;


        }


        // only start coroutine once, probably should be changed
        if (processingTurn) {
            return;
        }
        processingTurn = true;
        StartCoroutine(CalculateTurn());
    }


    ///////////////////////////
    /// debug
}
