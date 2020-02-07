using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : RoundIconButton {
    private bool paused = true;
    public Sprite playSprite;
    public Sprite pauseSprite;

    IEnumerator CheckForPauseStatus() {
        while (true) {
            if (GameManager.Paused != paused) {
                paused = GameManager.Paused;
                if (GameManager.Paused) {
                    image.sprite = playSprite;
                }
                else {
                    image.sprite = pauseSprite;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(CheckForPauseStatus());
    }

    // Update is called once per frame
    void Update() {

    }
}
