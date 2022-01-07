using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 ToVector3(this Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0f);
    }

    public static Vector3 ToVector3YisZ(this Vector2 vec)
    {
        return new Vector3(vec.x, 0f, vec.y);
    }

    public static Vector2 Invert(this Vector2 vec)
    {
        return new Vector2(vec.y, vec.x);
    }

    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }
}
