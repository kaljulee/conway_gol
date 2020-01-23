using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearButton : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        string fourColorShade = FourColor.LIGHTEST;
        image.color = new Color(
            FourColor.gameboyColorsRGB[fourColorShade][0],
            FourColor.gameboyColorsRGB[fourColorShade][1],
            FourColor.gameboyColorsRGB[fourColorShade][2]
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
