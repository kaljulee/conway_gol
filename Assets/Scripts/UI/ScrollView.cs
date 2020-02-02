using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    private Image[] images;
    private GameObject scrollbar;
    private Image viewBackground;
    // Start is called before the first frame update
    void Start()
    {
        //images = GetComponentsInChildren<Image>();

        scrollbar = transform.Find("Scrollbar Vertical").gameObject;
        images = scrollbar.GetComponentsInChildren<Image>();
        viewBackground = GetComponentsInChildren<Image>()[0];
        viewBackground.color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHT);

        
        images[0].color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.LIGHT);
        images[1].color = TwoBitColor.GenerateTwoBitColor(TwoBitColor.DARK);
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
