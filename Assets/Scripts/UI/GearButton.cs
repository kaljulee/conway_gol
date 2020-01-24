using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearButton : RoundIconButton
{

    new private void Awake()
    {
        fourColorShade = FourColor.LIGHTEST;
        base.Awake();

    }
}
