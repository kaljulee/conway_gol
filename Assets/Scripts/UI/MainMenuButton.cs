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
        
        string fourColorShade = FourColor.LIGHTEST;
        string textFourColorShade = FourColor.DARKEST;


        image.color = new Color(
    FourColor.gameboyColorsRGB[fourColorShade][0],
    FourColor.gameboyColorsRGB[fourColorShade][1],
    FourColor.gameboyColorsRGB[fourColorShade][2]
    );

        text.color = new Color(
    FourColor.gameboyColorsRGB[textFourColorShade][0],
    FourColor.gameboyColorsRGB[textFourColorShade][1],
    FourColor.gameboyColorsRGB[textFourColorShade][2]
            );

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
