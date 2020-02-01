using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {

    new void Awake() {
        base.Awake();
        //rectTransform.sizeDelta = new Vector2(208, 296);

        //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 296);
        //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 208);
    }

}
