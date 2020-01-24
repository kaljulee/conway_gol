using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public static GameObject mainMenu;
    public static ButtonManager instance = null;

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
        mainMenu.SetActive(false);
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
