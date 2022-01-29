using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtility
{
    public static ControlConfig controlConfig;

    public static bool AnyHeld()
    {
        return false;
    }
    public static bool EnterPressed()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }

        return false;
    }

    public static bool SpacePressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        return false;
    }

    public static bool EscapePressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }

        return false;
    }
}
