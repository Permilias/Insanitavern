using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector2 AddRot(Vector2 baseVector, float rotAdded)
    {
        float _rot = Mathf.Deg2Rad * rotAdded;
        return new Vector2((Mathf.Cos(_rot) * baseVector.x) - (Mathf.Sin(_rot) * baseVector.y), (Mathf.Sin(_rot) * baseVector.x) + (Mathf.Cos(_rot) * baseVector.y));
    }
}
