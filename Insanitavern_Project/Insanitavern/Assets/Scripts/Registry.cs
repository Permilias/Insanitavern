using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Registry
{
    public static GameSettings settings;

    public static List<Warrior> warriors;
    public static List<Mug> mugs;

    public static int GetMugIndexByWarrior(int index)
    {
        for (int i = 0; i < mugs.Count; i++)
        {
            if(mugs[i].holdingWarrior == index)
            {
                return i;
            }
        }

        Debug.Log("This warrior has no mug");
        return -1;
    }

    public static int GetWarriorIndexByPosition(Vector3 pos)
    {
        for (int i = 0; i < warriors.Count; i++)
        {
            if(warriors[i].movement.transform.position == pos)
            {
                return i;
            }
        }

        Debug.Log("No warrior by this position");
        return -1;
    }

    public static void Initialize()
    {
        warriors = new List<Warrior>();
        mugs = new List<Mug>();
    }


}
