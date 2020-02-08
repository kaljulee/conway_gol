using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundIconButton : MonoBehaviour {

    protected Image image;
    protected string fourColorShade;
    protected RectTransform rectTransform;
    protected float width = 30f;
    protected float height = 30f;
    public float size = 30f;

    //protected static Vector2 SetCustomSize(float w, float h)
    //{
    //    width = w;
    //    height = h;
    //}



    private void SetWidthAndHeight() {
        height = size;
        width = size;
    }

    public void SetSize(float newSize) {
        size = newSize;
        SetWidthAndHeight();
    }

    public virtual void Show() {
        gameObject.SetActive(true);
    }

    public virtual void Hide() {
        gameObject.SetActive(false);
    }

    protected void Awake() {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        SetWidthAndHeight();
        rectTransform.sizeDelta = new Vector2(width, height);

        if (fourColorShade == null) {
            fourColorShade = TwoBitColor.DARK;
        }
        image.color = TwoBitColor.GenerateTwoBitColor(fourColorShade);
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
