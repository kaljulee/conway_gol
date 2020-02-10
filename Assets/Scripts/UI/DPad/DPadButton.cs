using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPadButton : RoundIconButton
{

    public Vector3 direction;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    private Button button;



    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => { ButtonManager.instance.OnDPadButtonPress(direction); });
        if (direction == new Vector3(1, 0)) {
            image.sprite = rightSprite;
        }
        if (direction == new Vector3(0, 1)) {
            image.sprite = upSprite;
        }
        if (direction == new Vector3(-1, 0)) {
            image.sprite = leftSprite;
        }
        if (direction == new Vector3(0, -1)) {
            image.sprite = downSprite;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
