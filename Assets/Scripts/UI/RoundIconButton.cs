using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundIconButton : MonoBehaviour
{

    private Image image;
    protected string fourColorShade;

    protected void Awake()
    {
        image = GetComponent<Image>();
        if (fourColorShade == null)
        {
            fourColorShade = TwoBitColor.LIGHTEST;
        }
        image.color = TwoBitColor.GenerateTwoBitColor(fourColorShade);
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
