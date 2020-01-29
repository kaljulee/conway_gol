﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    // should be an object, dict or something
    
    public static readonly GameObject replacementOptions;

    public int ActionType { get; private set; }
    public float Payload { get; private set; }
    public Vector2? Address { get; private set; }

    public GameObject Target { get; private set; }

    private Action(int incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget)
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

    public Action ConvertToAddress()
    {
        if (Target == null)
        {
            return this;
        }
        return Factory.CreateAddressAction(ActionType, Payload, Target.transform.position);
    }
    public class Factory
    {

        private static Action CreateAction(int incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget)
        {
            return new Action(incomingType, incomingPayload, incomingAddress, incomingTarget);
        }

        public static Action CreateAddressAction(int incomingType, float incomingPayload, Vector2 incomingAddress)
        {
            return CreateAction(incomingType, incomingPayload, incomingAddress, null);
        }

        public static Action CreateDirectAction(int incomingType, float incomingPayload, GameObject incomingTarget)
        {
            return CreateAction(incomingType: incomingType, incomingPayload: incomingPayload, null, incomingTarget: incomingTarget);
        }
    }

    public static class ActionTypes
    {
        public const int PRESSURE_CHANGE = 0;
        public const int REMOVE = 1;
        public const int CREATE = 2;
    }

    public static class ZoneTypes
    {
        public static readonly int UNIT = 0;
        public static readonly int PRESSURE_ZONE = 1;

        public static int GetZoneType(Type type)
        {
            if (type == typeof(Unit))
            {
                return UNIT;
            }
            else if (type == typeof(PressureZone))
            {
                return PRESSURE_ZONE;
            }
            return -1;
        }
    }
}
