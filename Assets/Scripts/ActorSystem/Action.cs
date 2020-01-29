using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public static readonly string[] Types = { "PRESSURE_CHANGE", "REPLACE" };
    public static readonly GameObject replacementOptions;

    public string ActionType { get; private set; }
    public float Payload { get; private set; }
    public Vector2? Address { get; private set; }

    public GameObject Target { get; private set; }

    private Action(string incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget)
    {
        ActionType = incomingType;
        Payload = incomingPayload;
        Address = incomingAddress;
        Target = incomingTarget;
    }

    public override string ToString()
    {
        string returnValue = "action\ntype: " + ActionType + 
            "\npayload: " + Payload +
            "\naddress: ";
        if (Address != null)
        {
            Vector2 realAddress = (Vector2)Address;
            returnValue += ("x: " + realAddress.x + " y: " + realAddress.y);
        }
        else
        {
            returnValue += ("no address");
        }
        returnValue += "\ntarget: ";
        if (Target != null)
        {
            returnValue += ("target is real");
        } else
        {
            returnValue += "no target";
        }
        return returnValue;
    }
    public class Factory
    {

        private static Action CreateAction(string incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget)
        {
            return new Action(incomingType, incomingPayload, incomingAddress, incomingTarget);
        }

        public static Action CreateAddressAction(string incomingType, float incomingPayload, Vector2 incomingAddress)
        {
            return CreateAction(incomingType, incomingPayload, incomingAddress, null);
        }

        public static Action CreateDirectAction(string incomingType, float incomingPayload, GameObject incomingTarget)
        {
            return CreateAction(incomingType: incomingType, incomingPayload: incomingPayload, null, incomingTarget: incomingTarget);
        }
    }

}
