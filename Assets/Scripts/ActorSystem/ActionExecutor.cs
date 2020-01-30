using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExecutor : MonoBehaviour
{

    public static ActionExecutor instance = null;
    public static LinkedList<Action> history = new LinkedList<Action>();

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

    public void PushToHistory(Action action)
    {
        history.AddFirst(action.ConvertToAddress());
        //Debug.Log("--history after push--");
        //foreach (Action a in history)
        //{
        //    Debug.Log("   " + a.ToString());
        //}
    }
   
    public void ExecuteAction(Action action)
    {
        PushToHistory(action);
        GameManager.instance.ExecuteBoardAction(action);
        //return null;
    }

}
