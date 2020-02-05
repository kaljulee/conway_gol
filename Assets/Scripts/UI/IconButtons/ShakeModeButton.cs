using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeModeButton : RoundIconButton {
    private bool CurrentDrawMode = false;
    // Start is called before the first frame update
    void Start() {
        UpdateColor();
    }

    void UpdateColor() {
        bool managerDrawMode = ButtonManager.shakable;
        CurrentDrawMode = managerDrawMode;
        Color color = image.color;
        if (CurrentDrawMode) {
            color.a = 1f;
        }
        else {
            color.a = 0.5f;
        }
        image.color = color;
    }
    // Update is called once per frame
    void Update() {
        if (ButtonManager.shakable == CurrentDrawMode) { return; }
        UpdateColor();
    }
}
