using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundIconButton : MonoBehaviour
{

    private Image image;
    protected string fourColorShade;
    protected RectTransform rectTransform;
    protected float width = 30f;
    protected float height = 30f;

    //protected static Vector2 SetCustomSize(float w, float h)
    //{
    //    width = w;
    //    height = h;
    //}
    protected void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
        
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
