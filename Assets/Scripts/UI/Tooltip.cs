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
            Debug.Log("trying to fadein coroutine in " + text.text + " is active? " + gameObject.activeSelf);
            
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn() {
        Color newColor = text.color;
        Debug.Log("fade in start with a of " + newColor.a);
        while (newColor.a < 1) {
            Debug.Log("fade in step");
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
            Debug.Log("trying to fadeout coroutine in " + text.text + " is active? " + gameObject.activeSelf);

            StartCoroutine(FadeOut());
        }
    }

    void Start()
    {
        text = gameObject.GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        text.text = tooltipText;
        //if (!transform.parent.gameObject.activeSelf) {
        //gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
