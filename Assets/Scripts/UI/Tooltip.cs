using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Menu
{
    public string tooltipText = "tppooltipppppp";
    private Text text;
    public float tooltipWidth = 100;
    // Start is called before the first frame update


    public void Show() {
        if (transform.parent.gameObject.activeSelf) {
            gameObject.SetActive(true);
            
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn() {
        Color newColor = text.color;
        while (newColor.a < 1) {
            newColor.a += 0.1f;
            text.color = newColor;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOut() {
        Color newColor = text.color;
        while (newColor.a > 0) {
            newColor.a -= 0.1f;
            text.color = newColor;
            yield return new WaitForSeconds(0.15f);
        }
        gameObject.SetActive(false);
        yield return null;
    }
    public void Hide() {
        if (gameObject.activeSelf) {
            StartCoroutine(FadeOut());
        }
    }

    new void Awake()
    {
        base.Awake();
        text = gameObject.GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        text.text = tooltipText;
        if (!transform.parent.gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
