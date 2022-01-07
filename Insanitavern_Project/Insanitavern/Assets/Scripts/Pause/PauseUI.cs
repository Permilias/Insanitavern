using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject graphics;
}
