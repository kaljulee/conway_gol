using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject buttonManager;
    public GameObject actionController;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (ActionController.instance == null)
        {
            Instantiate(actionController);
        }
    }
}
