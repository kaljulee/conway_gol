using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject buttonManager;
    public GameObject actionExecutor;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (ActionExecutor.instance == null)
        {
            Instantiate(actionExecutor);
        }



        //if (ButtonManager.instance == null)
        //{
        //    Instantiate(buttonManager);
        //}
    }
}
