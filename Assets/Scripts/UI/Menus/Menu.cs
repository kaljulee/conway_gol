
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour {

    protected Image image;
    protected RectTransform rectTransform;


    virtual public bool ToggleActive() {
        bool newActiveState = !gameObject.activeSelf;
        gameObject.SetActive(newActiveState);
        return newActiveState;
    }

    virtual public void Close() {
        Tooltip[] tooltips = gameObject.GetComponentsInChildren<Tooltip>();
        foreach (Tooltip tooltip in tooltips) {
            tooltip.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    virtual public void Open() {
        gameObject.SetActive(true);
        Tooltip[] tooltips = gameObject.GetComponentsInChildren<Tooltip>();
        foreach (Tooltip tooltip in tooltips) {
            tooltip.gameObject.SetActive(false);

        }
    }

    public bool IsOpen() {
        return gameObject.activeSelf;
    }
    protected void Awake() {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        if (image) {
            string fourColorShade = TwoBitColor.LIGHT;
            Color color = TwoBitColor.GenerateTwoBitColor(fourColorShade);
            color.a = 0.2f;
            image.color = color;
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
