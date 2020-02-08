using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Action;
public class ButtonManager : MonoBehaviour {
    public static ButtonManager instance = null;

    public static GameObject mainMenu;
    public static MainMenu mainMenuScript;

    public static GameObject createRandomMenu;
    public static MainMenu createRandomScript;

    public static GameObject gameManager;
    public static GameManager gameManagerScript;
    public static int tooltipDelay = 3;

    public static GameObject slider;
    public static MainMenuSlider sliderScript;
    public static LinkedList<Menu> menus = new LinkedList<Menu>();

    public static GameObject templateMenu;
    public static MainMenu templateMenuScript;
    private static GameObject[] tooltips;
    private DrawPanel drawPanelScript;
    public int drawTool = ZoneTypes.UNIT;
    public LinkedList<Vector2> defaultDrawTemplate = Templates.Point();
    public LinkedList<Vector2> brickDrawTemplate = Templates.Point();

    private bool buttonReleased = false;
    private float holdTime = 0f;

    public static bool shakable { get; private set; }


    // gameobject functions
    private void Awake() {
        shakable = false;
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        mainMenuScript = mainMenu.GetComponent<MainMenu>();
        drawPanelScript = GameObject.FindGameObjectWithTag("DrawPanel").GetComponent<DrawPanel>();

        createRandomMenu = GameObject.FindGameObjectWithTag("CreateRandomMenu");
        createRandomScript = createRandomMenu.GetComponent<MainMenu>();

        templateMenu = GameObject.FindGameObjectWithTag("TemplateMenu");
        templateMenuScript = templateMenu.GetComponent<MainMenu>();

        slider = GameObject.FindGameObjectWithTag("Slider");
        sliderScript = slider.GetComponent<MainMenuSlider>();

        tooltips = GameObject.FindGameObjectsWithTag("Tooltip");

        menus.AddLast(mainMenuScript);
        menus.AddLast(createRandomScript);
        menus.AddLast(templateMenuScript);
        HideTooltips();
        CloseAllMenus();
        StartCoroutine(StartTooltips());
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void Update() {
        //Touch input = Input.GetTouch(0);
        //if (input.phase == TouchPhase.Began) {
        //    //OnGearButtonPress();
        //    Debug.Log("touch location " + input.position);
        //}
        if (!IsPointerOverUIObject()) {

            bool mouseDown = Input.GetMouseButton(0);
            if (mouseDown) {
                Vector2 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                position.y = Mathf.Round(position.y * Camera.main.orthographicSize * 2);
                position.x = Mathf.Round(position.x * Camera.main.aspect * Camera.main.orthographicSize * 2);
                if (!SomeMenuIsOpen()) {
                    Debug.Log("drawtool is " + ZoneTypes.ZONE_TYPE_STRINGS[drawTool]);
                    if (drawTool == ZoneTypes.UNIT) {
                        SpawnOnPoint(position, defaultDrawTemplate);
                    } else if (drawTool == ZoneTypes.BRICK) {
                        BrickOnPoint(position, brickDrawTemplate);
                    }
                }
            }
        }
        if (shakable && Input.acceleration.sqrMagnitude > 2) { OnShake(Input.acceleration.sqrMagnitude); }


    }

    private void ShowTooltips() {
        foreach (GameObject tooltip in tooltips) {
            tooltip.GetComponent<Tooltip>().Show();
        }
    }

    private void HideTooltips() {
        foreach (GameObject tooltip in tooltips) {
            if (tooltip.activeSelf) {
                tooltip.GetComponent<Tooltip>().Hide();
            }
        }
    }

    // utility functions
    IEnumerator StartTooltips() {
        ShowTooltips();
        for (int i = 0; i < tooltipDelay; i++) {
            yield return new WaitForSeconds(1);
        }
        HideTooltips();
        yield return null;
    }

    public void BrickOnPoint(Vector2 point, LinkedList<Vector2> spawn) {
        Debug.Log("BrickOnPoint");
        LinkedList<Vector2> zones = brickDrawTemplate;
        if (spawn != null) {
            zones = spawn;
        }
        GameManager.instance.SetSpawnCenter(point);
        GameManager.instance.RequestBrickZones(zones);
    }

    public void SpawnOnPoint(Vector2 point, LinkedList<Vector2> spawn) {

        LinkedList<Vector2> zones = defaultDrawTemplate;
        if (spawn != null) {
            zones = spawn;
        }
        GameManager.instance.SetSpawnCenter(point);
        GameManager.instance.RequestDrawZones(zones);
    }

    public void SetShakable(bool newShakable) {
        shakable = newShakable;
    }

    public void OnShake(float sqrMag) {
        ShakeOnPoint(sqrMag);
    }

    public void ShakeOnPoint(float sqrMag) {
        LinkedList<Vector2> zones = GameManager.instance.CreateRandomSpawnSites(sqrMag / 100); ;
        GameManager.instance.SetSpawnCenter(Vector2.zero);
        GameManager.instance.RequestShakeZones(zones);
    }

    public bool SomeMenuIsOpen() {
        foreach (Menu menu in menus) {
            if (menu.IsOpen()) {
                return true;
            }
        }
        return false;
    }

    public void CloseAllMenus() {
        foreach (MainMenu menu in menus) {
            menu.Close();
        }
    }

    ////////////////////////////
    /// major utility buttons
    /// 
    public void OnGearButtonPress() {

        if (SomeMenuIsOpen()) {
            CloseAllMenus();
        }
        else {
            mainMenuScript.Open();
        }

        if (mainMenuScript.IsOpen()) {
            GameManager.SetPaused();
        }
        else {
            GameManager.SetUnPaused();
        }

    }

    public void OnHelpButtonPress() {
        StartCoroutine(StartTooltips());
    }


    public void OnExitButtonPress(GameObject menu) {
        menu.SetActive(false);
        Debug.Log("exit button pressed!");
    }

    public void OnClearPress() {
        GameManager.instance.ClearBoard();
    }

    public void OnShakeTogglePress() {
        SetShakable(!shakable);
    }



    ////////////////////////////
    /// main menu buttons
    /// 

    public void OnResetButtonPress() {
        gameManagerScript.ResetGameState();
    }

    public void OnCreateRandomPress() {
        mainMenu.SetActive(false);
        createRandomMenu.SetActive(true);
        GameManager.instance.ApplyRandomSpawnSites(sliderScript.GetValue());
        ActionController.instance.ClearHistory();
    }

    public void OnReRollRandomPress() {
        GameManager.instance.ApplyRandomSpawnSites(sliderScript.GetValue());
        ActionController.instance.ClearHistory();
    }

    public void OnRunRandomPress() {
        CloseAllMenus();
        GameManager.SetUnPaused();
    }
    public void OnTemplatesPress() {
        mainMenu.SetActive(false);
        templateMenu.SetActive(true);
    }

    public void OnTemplateSelectorPress(int template) {
        Action templateAction = Action.Factory.CreateAddressAction(Action.ActionTypes.SET_TEMPLATE, template, Vector2.zero);
        ActionController.instance.ExecuteAction(templateAction);
        defaultDrawTemplate = Templates.GetTemplate(template);
        GameManager.instance.SetDrawMode(true);
        CloseAllMenus();
    }

    ////////////////////////////
    /// player buttons
    /// 
    public void OnPlayPress() {
        if (GameManager.Paused) {
            GameManager.SetUnPaused();
        }
        else {
            GameManager.SetPaused();
        }
    }

    private void SafetyPause() {
        if (!GameManager.Paused) {
            GameManager.SetPaused();
        }
    }
    public void OnStopPress() {
        GameManager.SetPaused();
    }

    public void OnStepForwardHold() {
        SafetyPause();
        GameManager.instance.StartStepping(1);
    }

    public void OnStepForwardRelease() {
        GameManager.instance.StopStepping();
    }

    public void OnStepBackwardHold() {
        SafetyPause();
        GameManager.instance.StartStepping(-1);
    }

    public void onStepBackwardRelease() {
        GameManager.instance.StopStepping();
    }

    IEnumerator ButtonHoldTimer(System.Func<float, bool> releaseSchedule) {
        holdTime = Time.time;
        bool actionTaken = false;

        while (!actionTaken) {
            actionTaken = releaseSchedule(holdTime);
            yield return null;
        }
        yield return null;
    }

    public void OnDrawButtonHold() {
        buttonReleased = false;
        holdTime = Time.time;
        StartCoroutine(ButtonHoldTimer((float time) => {
            float elapsedTime = Time.time - time;
            if (elapsedTime > 0.5f) {
                drawPanelScript.ToggleOpen();
                return true;
            }
            if (buttonReleased) {
                if (drawPanelScript.isOpen) {
                    drawPanelScript.CollapsePanel();
                } else {
                    GameManager.instance.ToggleDrawMode();

                }
                return true;
            }
            return false;
        }));
    }

    public void OnDrawButtonRelease() {
        buttonReleased = true;
    }


    private void DrawButtonPress(int zoneType) {
        Debug.Log("in generic drawbutton press");
        drawTool = zoneType;
        Debug.Log("drawtool is now " + ZoneTypes.ZONE_TYPE_STRINGS[drawTool]);
        GameManager.instance.SetDrawMode(true);
        drawPanelScript.CollapsePanel();
    }

    public void OnDrawUnitPress() {
        DrawButtonPress(ZoneTypes.UNIT);
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////
    /// </summary>
    // this needs to be involved in a hold-to-open with draw menu and better defined draw button options
    public void OnDrawBrickPress() {
        DrawButtonPress(ZoneTypes.BRICK);
    }

    public void OnDrawErasePress() {
        Debug.Log("not implemented yet");
    }
}
