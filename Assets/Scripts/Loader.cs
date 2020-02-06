using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject buttonManager;
    public GameObject actionController;
    public Camera mainCamera;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (ActionController.instance == null)
        {
            Instantiate(actionController);
        }
    }

    private void Start() {
    }
}
