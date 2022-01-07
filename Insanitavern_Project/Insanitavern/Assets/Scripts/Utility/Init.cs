using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Init : MonoBehaviour
{
    public bool reset;

    public GameSettings settings;
    public ControlConfig controlConfig;
    public FXConfig fxConfig;
    public SoundConfig soundConfig;

    private void Start()
    {
        Debug.Log("Initializing...");

        DataManager.SetPath();
        DataManager.Load();

        if(reset)
        {
            DataManager.Reset();
        }

        if(GameManager.willReset)
        {
            GameManager.willReset = false;
            DataManager.Reset();
        }

        Registry.settings = settings;

        DataManager.data.fullscreen = false;

        InputUtility.controlConfig = controlConfig;

        FXManager.config = fxConfig;
        FXManager.Initialize();

        SoundManager.config = soundConfig;
        SoundManager.Initialize();

        GameManager.Initialize();

        GameManager.LoadScene("Main");
    }
}
