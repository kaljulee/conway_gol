using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{

    public static ActionController instance = null;
    public static LinkedList<LinkedList<Action>> history = new LinkedList<LinkedList<Action>>();
    private bool rewinding = false;

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
        bool historicalAction = action.ActionType == Action.ActionTypes.CREATE || action.ActionType == Action.ActionTypes.REMOVE;
        if (historicalAction && !rewinding) {
            PushToHistory(action);
        }
        GameManager.instance.ExecuteBoardAction(action);
    }

    private void UndoAction(Action action) {
        GameManager.instance.ExecuteBoardAction(Action.Invert(action));
    }

    public void Rewind() {
        if (history.Count == 0) return;
        rewinding = true;
        LinkedList<Action> round = history.First.Value;
        Debug.Log("round being rewound:");
        //foreach(Action action in round) {
        //    Debug.Log("the action///////////////");
        //    Debug.Log("   " + action.ToString());
        //    Debug.Log("inverted");
        //    Debug.Log(Action.Invert(action).ToString());
        //    Debug.Log("---------------");

        //}
        history.RemoveFirst();
        foreach (Action action in round) {
            Debug.Log("the action///////////////");
            Debug.Log("   " + action.ToString());
            Debug.Log("inverted");
            Debug.Log(Action.Invert(action).ToString());
            Debug.Log("---------------");
            UndoAction(action);
        }
        GameManager.instance.ExecuteBoardAction(Action.Factory.CreateAddressAction(Action.ActionTypes.ALL_PRESSURE_ZERO, 0, Vector2.zero));
        rewinding = false;
    }

}
