using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    public static Vector2 _v2FromV3(Vector3 v) => new Vector2(v.x, v.y);

    public static bool VectorCheck(Vector2 v1, Vector2 v2) {
        return Mathf.Round(v1.x) == Mathf.Round(v2.x) && Mathf.Round(v1.y) == Mathf.Round(v2.y);
    }

    public static bool VectorCheck(Vector2 v1, Vector3 v2) => VectorCheck(v1, _v2FromV3(v2));

    public static bool VeectorCheck(Vector3 v1, Vector2 v2) => VectorCheck(_v2FromV3(v1), v2);


}
