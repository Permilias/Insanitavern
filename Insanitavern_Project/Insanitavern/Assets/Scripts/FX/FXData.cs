using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FXData
{
    public string fxName;

    public GameObject fx;
    public int poolingAmount;

    public float duration;

    public bool randomizeSize;
    public Vector2 minMaxSize;

    public bool randomizeRotation;
}
