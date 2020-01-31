using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    public static ActionController instance = null;
    public static LinkedList<LinkedList<Action>> history = new LinkedList<LinkedList<Action>>();
    private bool rewinding = false;

    public void ClearHistory() {
        history.Clear();
    }
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void BeginNewRound() {
        history.AddFirst(new LinkedList<Action>());
    }

    public void EndRound() {
        if (history.First.Value.Count == 0) {
            history.RemoveFirst();
        }
    }

    public void PushToHistory(Action action) {
        history.First.Value.AddFirst(action.ConvertToAddress());
    }

    public void ExecuteAction(Action action) {
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

        // fetch and drop first history entry
        LinkedList<Action> round = history.First.Value;
        history.RemoveFirst();


        // undo historical actions
        foreach (Action action in round) {
            UndoAction(action);
        }

        // clean up pressures
        GameManager.instance.ExecuteBoardAction(Action.Factory.CreateAddressAction(Action.ActionTypes.ALL_PRESSURE_ZERO, 0, Vector2.zero));

        rewinding = false;
    }

}
