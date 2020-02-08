using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsablePanel : MonoBehaviour
{
    RectTransform rectTransform;
    RoundIconButton[] iconButtons;
    public float expansionTime = 0.2f;
    public float contractionTime = 0.2f;
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

    IEnumerator Expander() {
        ShowChildButtons();
        rectTransform.sizeDelta = new Vector2(120, rectTransform.sizeDelta.y);
        yield return null;
    }

    IEnumerator Collapser() {
        rectTransform.sizeDelta = new Vector2(30, rectTransform.sizeDelta.y);
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
