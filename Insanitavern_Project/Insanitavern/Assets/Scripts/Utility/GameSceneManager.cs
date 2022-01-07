using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    private void Awake()
    {
        if (!GameManager.init)
        {
            Debug.Log("Not Initialized !");
            GameManager.LoadInit();
            return;
        }

        InstanceSetting();
    }

    public virtual void InstanceSetting()
    {

    }

    private void Start()
    {
        if(GameManager.init)
        Initialize();
    }

    public virtual void Initialize()
    {
        DataManager.CreateSceneDataIfMissing();
        MuteButton.Instance.Initialize();
        SavedItemManager.Initialize();
        CinematicBars.Instance.Depop();
    }
}
