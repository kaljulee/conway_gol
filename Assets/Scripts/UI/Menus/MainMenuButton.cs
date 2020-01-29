using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{

    private Image image;
    private Text text;

    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        
        string fourColorShade = TwoBitColor.LIGHTEST;
        string textFourColorShade = TwoBitColor.DARKEST;


        image.color = TwoBitColor.GenerateTwoBitColor(fourColorShade);

        text.color = TwoBitColor.GenerateTwoBitColor(textFourColorShade);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
