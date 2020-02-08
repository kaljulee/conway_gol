using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {
    // should be an object, dict or something

    public static readonly GameObject replacementOptions;

    public int ActionType { get; private set; }
    public float Payload { get; private set; }
    public Vector2? Address { get; private set; }

    public GameObject Target { get; private set; }

    private Action(int incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget) {
        ActionType = incomingType;
        Payload = incomingPayload;
        Address = incomingAddress;
        Target = incomingTarget;
    }

    public static Action PileOn(Action action, float value) {
        if (action.Target) {
            return Factory.CreateDirectAction(action.ActionType, action.Payload + value, action.Target);
        }
        return Factory.CreateAddressAction(action.ActionType, action.Payload + value, (Vector2)action.Address);
    }

    public override string ToString() {
        string returnValue = "action type: " + ActionTypes.ACTION_TYPE_STRINGS[ActionType] +
            " payload: " + Payload +
            " address: ";
        if (Address != null) {
            Vector2 realAddress = (Vector2)Address;
            returnValue += ("x: " + realAddress.x + " y: " + realAddress.y);
        }
        else {
            returnValue += ("no address");
        }
        returnValue += " target: ";
        if (Target != null) {
            returnValue += ("target is real and at x" + Target.transform.position.x + " y" + Target.transform.position.y);
        }
        else {
            returnValue += "no target";
        }
        returnValue += "////end of action///";
        return returnValue;
    }

    public Action ConvertToAddress() {
        if (Target == null) {
            return this;
        }
        return Factory.CreateAddressAction(ActionType, Payload, Target.transform.position);
    }

    public static Action Invert(Action action) {
        switch (action.ActionType) {
            case ActionTypes.CREATE:
                if (action.Address != null) {
                    return Factory.CreateAddressAction(ActionTypes.REMOVE, action.Payload, (Vector2)action.Address);
                }
                if (action.Target != null) {
                    return Factory.CreateAddressAction(ActionTypes.REMOVE, action.Payload, action.Target.transform.position);
                }
                return null;
            case ActionTypes.PRESSURE_CHANGE:
                int invertedPressure = (int)Mathf.Round(action.Payload) * -1;
                if (action.Address != null) {
                    return Factory.CreateAddressAction(ActionTypes.PRESSURE_CHANGE, invertedPressure, (Vector2)action.Address);
                }
                if (action.Target != null) {
                    return Factory.CreateDirectAction(ActionTypes.PRESSURE_CHANGE, invertedPressure, action.Target);
                }
                return null;
            // pressure in created is enough to maintain the zone type
            case ActionTypes.REMOVE:
                int pressure = 1;
                if (action.Payload == ZoneTypes.UNIT) {
                    pressure = 3;
                }
                if (action.Address != null) {
                    return Factory.CreateAddressAction(ActionTypes.CREATE, pressure, (Vector2)action.Address);
                }
                if (action.Target != null) {
                    return Factory.CreateDirectAction(ActionTypes.CREATE, action.Payload, action.Target);
                }
                return null;
            case ActionTypes.PRESSURE_ZERO:
                if (action.Target != null) {
                    return Factory.CreateDirectAction(ActionTypes.PRESSURE_CHANGE, action.Payload, action.Target);
                }
                if (action.Address != null) {
                    return Factory.CreateAddressAction(ActionTypes.PRESSURE_CHANGE, action.Payload, (Vector2)action.Address);
                }
                return null;
            default:
                return null;
        }
    }

    public class Factory {

        private static Action CreateAction(int incomingType, float incomingPayload, Vector2? incomingAddress, GameObject incomingTarget) {
            return new Action(incomingType, incomingPayload, incomingAddress, incomingTarget);
        }

        public static Action CreateAddressAction(int incomingType, float incomingPayload, Vector2 incomingAddress) {
            return CreateAction(incomingType, incomingPayload, incomingAddress, null);
        }

        public static Action CreateDirectAction(int incomingType, float incomingPayload, GameObject incomingTarget) {
            return CreateAction(incomingType: incomingType, incomingPayload: incomingPayload, null, incomingTarget: incomingTarget);
        }
    }

    public static class ActionTypes {
        public const int PRESSURE_CHANGE = 0;
        public const int REMOVE = 1;
        public const int CREATE = 2;
        public const int PRESSURE_ZERO = 3;
        public const int ALL_PRESSURE_ZERO = 4;
        public const int SET_TEMPLATE = 5;
        public const int CLEAR_ALL = 6;
        public static readonly string[] ACTION_TYPE_STRINGS = { "PRESSURE_CHANGE", "REMOVE", "CREATE", "PRESSURE_ZERO", "ALL_PRESSURE_ZERO", "SET_TEMPLATE", "CLEAR_ALL" };
    }

    public static class ZoneTypes {
        public static readonly int UNIT = 0;
        public static readonly int PRESSURE_ZONE = 1;
        public static readonly int BRICK = 2;
        public static readonly string[] ZONE_TYPE_STRINGS = { "UNIT", "PRESSURE_ZONE", "BRICK" };

        public static int GetZoneType(PressureZone zone) {
            if (zone is Unit) {
                return UNIT;
            }
            if (zone is Brick) {
                return BRICK;
            }
            return PRESSURE_ZONE;
        }
    }
}
