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
        history.AddFirst(action);
    }
   
    public GameObject ExecuteAction(Action action)
    {

        return null;
    }

}
