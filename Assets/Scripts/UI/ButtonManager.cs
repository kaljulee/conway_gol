﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance = null;

    public static GameObject mainMenu;
    public static MainMenu mainMenuScript;

    public static GameObject createRandomMenu;
    public static MainMenu createRandomScript;

    public static GameObject gameManager;
    public static GameManager gameManagerScript;

    public static GameObject slider;
    public static MainMenuSlider sliderScript;
    public static LinkedList<Menu> menus = new LinkedList<Menu>();

    private void Awake()
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
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        mainMenuScript = mainMenu.GetComponent<MainMenu>();

        createRandomMenu = GameObject.FindGameObjectWithTag("CreateRandomMenu");
        createRandomScript = createRandomMenu.GetComponent<MainMenu>();


        slider = GameObject.FindGameObjectWithTag("Slider");
        sliderScript = slider.GetComponent<MainMenuSlider>();

        menus.AddLast(mainMenuScript);
        menus.AddLast(createRandomScript);
        CloseAllMenus();
    }

    public bool SomeMenuIsOpen() {
        foreach(Menu menu in menus) {
            if (menu.IsOpen()) {
                return true;
            }
        }
        return false;
    }

    public void CloseAllMenus() {
        foreach(MainMenu menu in menus) {
            menu.Close();
        }
    }

    ////////////////////////////
    /// major utility buttons
    /// 
    public void OnGearButtonPress()
    {

        if (SomeMenuIsOpen()) {
            CloseAllMenus();
        } else {
            mainMenuScript.Open();
        }

        if (mainMenuScript.IsOpen())
        {
            GameManager.SetPaused();
        } else
        {
            GameManager.SetUnPaused();
        }

    }

    public void OnExitButtonPress()
    {
        Debug.Log("exit button pressed!");
    }


    ////////////////////////////
    /// main menu buttons
    /// 

    public void OnResetButtonPress()
    {
        gameManagerScript.ResetGameState();
    }


    public void OnCreateRandomPress()
    {
        mainMenu.SetActive(false);
        createRandomMenu.SetActive(true);
        GameManager.instance.ApplyRandomSpawnSites(sliderScript.GetValue());
    }

    public void OnReRollRandomPress() {
        GameManager.instance.ApplyRandomSpawnSites(sliderScript.GetValue());
    }

    public void OnRunRandomPress() {
        CloseAllMenus();
        GameManager.SetUnPaused();
    }
    public void OnTemplatesPress()
    {

    }

    ////////////////////////////
    /// player buttons
    /// 
    public void OnPlayPress()
    {
        GameManager.SetUnPaused();
    }

    public void OnPausePress() {
        GameManager.SetPaused();
    }

    private void SafetyPause() {
        if (!GameManager.Paused) {
            GameManager.SetPaused();
        }
    }
    public void OnStopPress()
    {
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
}
