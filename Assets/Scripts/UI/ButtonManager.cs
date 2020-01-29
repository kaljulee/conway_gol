using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public static GameObject mainMenu;
    public static ButtonManager instance = null;
    public static GameObject gameManager;
    public static GameManager gameManagerScript;
    public static GameObject mainMenuSlider;
    public static MainMenu mainMenuScript;

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
        mainMenu = GameObject.Find("MainMenu");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mainMenuSlider = GameObject.FindGameObjectWithTag("MainMenuSlider");

        mainMenuScript = mainMenu.GetComponent<MainMenu>();
        mainMenu.SetActive(false);
    }

    ////////////////////////////
    /// major utility buttons
    /// 
    public void OnGearButtonPress()
    {
        mainMenuScript.ToggleActive();
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


    public void OnRandomGamePress()
    {
        float frequency = mainMenuSlider.GetComponent<MainMenuSlider>().GetValue();
        GameManager.instance.ApplyRandomSpawnSites(frequency);
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

    public void OnStopPress()
    {
        GameManager.SetPaused();
    }

    public void OnStepForwardPress()
    {
        Debug.Log("forward step pressed");
    }

}
