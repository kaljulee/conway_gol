using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Menu
{
    public string tooltipText = "tppooltipppppp";
    private Text text;
    // Start is called before the first frame update


    void Start()
    {
        text = gameObject.GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        Debug.Log("///////////");
        Debug.Log("origininal text: " + text.text);
        Debug.Log("supplied text: " + tooltipText);
        text.text = tooltipText;
        Debug.Log("new text: " + text.text);
        Color newColor = image.color;
        newColor.a = 1;
        image.color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
