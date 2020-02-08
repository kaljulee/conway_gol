using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsablePanel : MonoBehaviour
{
    RectTransform rectTransform;
    RoundIconButton[] iconButtons;
    public float expansionTime = 0.01f;
    public float contractionTime = 0.005f;
    public float expandedWidth = 120f;
    public float collapsedWidth = 30f;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        iconButtons = GetComponentsInChildren<RoundIconButton>();

        HideChildButtons();
        rectTransform.sizeDelta = new Vector2(30, rectTransform.sizeDelta.y);
        isOpen = false;
        Debug.Log("draw panel should have hid");
    }

    private void HideChildButtons() {
        for (int i = 0; i < iconButtons.Length - 1; i++) {
            iconButtons[i].Hide();
        }
    }

    private void ShowChildButtons() {
        for (int i = 0; i < iconButtons.Length - 1; i++) {
            iconButtons[i].Show();
        }
    }

    public void ToggleOpen() {
        isOpen = !isOpen;
        if (isOpen) {
            ExpandPanel();
        } else {
            CollapsePanel();
        }
    }

    private void IncrementWidth(float increment) {
        float newWidth = rectTransform.sizeDelta.x + increment;
        if (newWidth > expandedWidth) {
            newWidth =  expandedWidth;
        }
        if (newWidth < collapsedWidth) {
            newWidth =  collapsedWidth;
        }
        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
    }

    IEnumerator Expander() {
        ShowChildButtons();
        while(rectTransform.sizeDelta.x < expandedWidth) {
            IncrementWidth(5f);
            yield return new WaitForSeconds(expansionTime);
        }
        yield return null;
    }

    IEnumerator Collapser() {
        while (rectTransform.sizeDelta.x > collapsedWidth) {
            IncrementWidth(-10f);
            yield return new WaitForSeconds(contractionTime);
        }
        HideChildButtons();
        yield return null;
    }

    public void ExpandPanel() {
        StartCoroutine(Expander());
    }

    public void CollapsePanel() {
        StartCoroutine(Collapser());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
