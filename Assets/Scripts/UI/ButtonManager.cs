using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public static GameObject mainMenu;
    public static ButtonManager instance = null;
    public static GameObject gameManager;

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
        
        mainMenu.SetActive(false);
    }

    public void OnResetButtonPress()
    {
        gameManager.GetComponent<GameManager>().ResetState();
    }
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

}
