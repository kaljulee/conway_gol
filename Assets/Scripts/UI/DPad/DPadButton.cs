using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPadButton : RoundIconButton
{

    public Vector2 direction;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (direction == new Vector2(1, 0)) {
            image.sprite = rightSprite;
        }
        if (direction == new Vector2(0, 1)) {
            image.sprite = upSprite;
        }
        if (direction == new Vector2(-1, 0)) {
            image.sprite = leftSprite;
        }
        if (direction == new Vector2(0, -1)) {
            image.sprite = downSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
