using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtility
{
    public static ControlConfig controlConfig;

    public static bool AnyHeld()
    {
        if (LeftHeld()) return true;
        if (RightHeld()) return true;
        if (UpHeld()) return true;
        if (DownHeld()) return true;

        return false;
    }

    public static bool LeftHeld()
    {
        for (int i = 0; i < controlConfig.leftInputs.Length; i++)
        {
            if(Input.GetKey(controlConfig.leftInputs[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool RightHeld()
    {
        for (int i = 0; i < controlConfig.rightInputs.Length; i++)
        {
            if (Input.GetKey(controlConfig.rightInputs[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool LeftPressed()
    {
        for (int i = 0; i < controlConfig.leftInputs.Length; i++)
        {
            if (Input.GetKeyDown(controlConfig.leftInputs[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool RightPressed()
    {
        for (int i = 0; i < controlConfig.rightInputs.Length; i++)
        {
            if (Input.GetKeyDown(controlConfig.rightInputs[i]))
            {
                return true;
            }
        }

        return false;
    }
    public static bool UpHeld()
    {
        for (int i = 0; i < controlConfig.upInputs.Length; i++)
        {
            if (Input.GetKey(controlConfig.upInputs[i]))
            {
                return true;
            }
        }

        return false;
    }
    public static bool DownHeld()
    {
        for (int i = 0; i < controlConfig.downInputs.Length; i++)
        {
            if (Input.GetKey(controlConfig.downInputs[i]))
            {
                return true;
            }
        }

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
