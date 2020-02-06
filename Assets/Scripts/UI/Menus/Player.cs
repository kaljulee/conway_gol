using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Menu
{
    private GameObject playButton;
    // Start is called before the first frame update

    new void Awake() {
        base.Awake();
        gameObject.SetActive(true);
    }

    void Start()
    {
        //playButton = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //playButton.SetActive(GameManager.Paused);
    }
}
