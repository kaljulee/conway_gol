using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action<T>
{

    public static readonly string[] Types = { "PRESSURE_CHANGE", "DESTROY", "REPLACE" };

    public string actionType { get; private set; }
    public T payload { get; private set; }
    public Vector2 address { get; private set; }

    Action(string incomingType, T incomingPayload, Vector2 incomingAddress)
    {
        actionType = incomingType;
        payload = incomingPayload;
        address = incomingAddress;
    }

}
