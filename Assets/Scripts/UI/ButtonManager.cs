using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public static GameObject mainMenu;
    public static ButtonManager instance = null;
    public static GameObject gameManager;
    public static GameObject mainMenuSlider;

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
        Debug.Log("gamemanager is null? " + (gameManager == null));
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        Debug.Log("gamemanager is null? " + (gameManager == null));
        mainMenuSlider = GameObject.FindGameObjectWithTag("MainMenuSlider");
        
        mainMenu.SetActive(false);
    }

    ////////////////////////////
    /// major utility buttons
    /// 
    public void OnGearButtonPress()
    {
        GameManager.TogglePaused();
        mainMenu.GetComponent<MainMenu>().ToggleActive();
        Debug.Log("gear button pressed!");
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
        gameManager.GetComponent<GameManager>().ResetGameState();
    }


    public void OnRandomGamePress()
    {
        float frequency = mainMenuSlider.GetComponent<MainMenuSlider>().GetValue();
        gameManager.GetComponent<GameManager>().ApplyRandomSpawnSites(frequency);
    }

    public void OnTemplatesPress()
    {

    }

    ////////////////////////////
    /// player buttons
    /// 
    public void OnPlayPress()
    {

    }

    public void OnStopPress()
    {

    }

    public void OnStepForwardPress()
    {

    }

}
