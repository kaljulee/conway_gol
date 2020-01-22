using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{

    public static Dictionary<string, Func<Vector2, Vector2>> directionFromCenter = new Dictionary<string, Func<Vector2, Vector2>> {
            { "up", up },
            {"down", down },
            {"left", left },
            {"right", right },
            {"upLeft", upLeft },
            {"upRight", upRight },
            {"downLeft", downLeft },
            {"downRight", downRight }
    };
    private static Vector2 incrementVector(Vector2 originalVector, int x, int y)
    {
        return originalVector + new Vector2(x, y);
    } 

    public static Vector2 up(Vector2 originalVector)
    {
        return incrementVector(originalVector, 0, 1);
    }
    public static Vector2 down(Vector2 originalVector)
    {
        return incrementVector(originalVector, 0, -1);
    }

    public static Vector2 left(Vector2 originalVector)
    {
        return incrementVector(originalVector, -1, 0);
    }
    public static Vector2 right(Vector2 originalVector)
    {
        return incrementVector(originalVector, 1, 0);
    }

    public static Vector2 upRight(Vector2 originalVector)
    {
        return incrementVector(originalVector, 1, 1);
    }
    public static Vector2 upLeft(Vector2 originalVector)
    {
        return incrementVector(originalVector, -1, 1);
    }

    public static Vector2 downRight(Vector2 originalVector)
    {
        return incrementVector(originalVector, 1, -1);
    }
    public static Vector2 downLeft(Vector2 originalVector)
    {
        return incrementVector(originalVector, -1, -1);
    }
}
