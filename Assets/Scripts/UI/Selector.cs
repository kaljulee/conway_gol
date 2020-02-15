using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : PlaceholderGameboyColored {
    private string firstShade = TwoBitColor.LIGHT;
    private string secondShade = TwoBitColor.LIGHTEST;
    private LinkedList<string> shades = new LinkedList<string>();

    IEnumerator AlternateShades() {
        LinkedListNode<string> node = shades.First;
        while (true) {
            ColorPlaceholder(node.Value);
            if (node.Next == null) {
                node = shades.First;
            } else {
                node = node.Next;
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    private void OnEnable() {
        StartCoroutine(AlternateShades());
    }

    new void Awake() {
        shades.AddLast(firstShade);
        shades.AddLast(secondShade);
        colorShade = shades.First.Value;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AlternateShades());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
