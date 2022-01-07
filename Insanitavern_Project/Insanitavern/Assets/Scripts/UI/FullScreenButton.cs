using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenButton : MonoBehaviour
{
    public static FullScreenButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    void SetFullScreen(bool state)
    {
        Screen.SetResolution(state ? 1920 : 980, state ? 1080 : 551, state ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    public void ToggleFullScreen()
    {
        DataManager.data.fullscreen = !DataManager.data.fullscreen;
        SetFullScreen(DataManager.data.fullscreen);
    }
}
