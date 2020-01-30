using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{

    public static ActionController instance = null;
    public static LinkedList<LinkedList<Action>> history = new LinkedList<LinkedList<Action>>();
    

    public void ClearHistory()
    {
        history.Clear();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void BeginNewRound() {
        history.AddFirst(new LinkedList<Action>());
    }

    public void PushToHistory(Action action)
    {
        history.First.Value.AddFirst(action.ConvertToAddress());
    }
   
    public void ExecuteAction(Action action)
    {
        PushToHistory(action);
        GameManager.instance.ExecuteBoardAction(action);
    }

    private void UndoAction(Action action) {
        GameManager.instance.ExecuteBoardAction(Action.Invert(action));
    }

    public void Rewind() {
        if (history.Count == 0) return;
        LinkedList<Action> round = history.First.Value;
        Debug.Log("round being rewound:");
        foreach(Action action in round) {
            Debug.Log("   " + action.ToString());
        }
        history.RemoveFirst();
        foreach (Action action in round) {
            UndoAction(action);
        }
    }

}
