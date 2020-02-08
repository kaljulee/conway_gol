﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public LinkedList<Vector2> defaultDrawTemplate = Templates.Point();

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
        Debug.Log("raycasting with result count of " + results.Count);
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
                    SpawnOnPoint(position, defaultDrawTemplate);
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

    public void OnDrawUnitPress() {

    }

    public void OnDrawBrickPress() {

    }

    public void OnDrawErasePress() {

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

    public void OnDrawButtonPress() {
        GameManager.instance.ToggleDrawMode();
    }
}
