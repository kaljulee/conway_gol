using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Menu
{
    public string tooltipText = "tppooltipppppp";
    private Text text;
    public float tooltipWidth = 100;
    // Start is called before the first frame update


    public void Show() {
        if (transform.parent.gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    void Start()
    {
        text = gameObject.GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        text.text = tooltipText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
