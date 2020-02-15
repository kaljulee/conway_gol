using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMenuControls : Menu
{

    public bool selectorActive = false;
    public override void Close() {
        selectorActive = false;
        base.Close();
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
