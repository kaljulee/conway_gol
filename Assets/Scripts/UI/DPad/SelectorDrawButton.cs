using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorDrawButton : ControllerActionButton
{
    public Sprite unitSprite;
    public Sprite eraseSprite;
    public Sprite brickSprite;
    private int localDrawTool;

    public void UpdateButtonSprite(int toolInt) {
        image.sprite = null;
        if (toolInt == Action.ZoneTypes.BRICK) {
            image.sprite = brickSprite;
        }
        else if (toolInt == Action.ZoneTypes.ERASE) {
            image.sprite = eraseSprite;
        }
        else if (toolInt == Action.ZoneTypes.UNIT) {
            image.sprite = unitSprite;
        }
        localDrawTool = toolInt;
    }

    IEnumerator CheckDrawTool() {
        while (true) {
            int actualDrawTool = ButtonManager.instance.drawTool;

            if (actualDrawTool != localDrawTool) {
                Debug.Log("values are different, updating sprite, local: " + localDrawTool + " actual: " + actualDrawTool);
                UpdateButtonSprite(actualDrawTool);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int toolInt = ButtonManager.instance.drawTool;
        UpdateButtonSprite(toolInt);
        StartCoroutine(CheckDrawTool());

    }

    private void OnEnable() {
        StartCoroutine(CheckDrawTool());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
