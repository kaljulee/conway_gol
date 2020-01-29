using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSlider : MonoBehaviour
{
    private Slider slider;
    private Image[] images;

    public float GetValue()
    {
        return slider.value;
    }
    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        images = slider.GetComponentsInChildren<Image>();
        // background
        images[0].color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHTEST);
        // fill
        images[1].color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.DARK);
        // handle
        images[2].color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHT);
        
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
