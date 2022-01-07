using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    public static MuteButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject mutedSprite, unMutedSprite;

    public void Initialize()
    {
        SetMute(DataManager.data.muted);
    }

    void SetMute(bool state)
    {
        AudioListener.volume = state ? 0f : 1f;
        mutedSprite.SetActive(state);
        unMutedSprite.SetActive(!state);
    }

    public void ToggleMute()
    {
        DataManager.data.muted = !DataManager.data.muted;
        SetMute(DataManager.data.muted);
    }
}
