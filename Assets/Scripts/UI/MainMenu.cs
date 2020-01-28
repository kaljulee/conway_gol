using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Image image;
    public bool ToggleActive()
    {
        bool newActiveState = !gameObject.activeSelf;
        gameObject.SetActive(newActiveState);
        return newActiveState;
    }
    private void Awake()
    {
        image = GetComponent<Image>();
        string fourColorShade = TwoBitColor.LIGHT;
        Color color = TwoBitColor.GenerateTwoBitColor(fourColorShade);
        color.a = 0.2f;
        image.color = color;
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
