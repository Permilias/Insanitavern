using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public List<AudioClip> clips;
    [Range(0, 1)]
    public float minVolume = 1;
    [Range(0, 1)]
    public float maxVolume = 1;
    [Range(0, 2)]
    public float minPitch = 1;
    [Range(0, 2)]
    public float maxPitch = 1;

    public Sound(List<AudioClip> _clips, float _minVolume, float _maxVolume, float _minPitch, float _maxPitch)
    {
        clips = _clips;
        minVolume = _minVolume;
        maxVolume = _maxVolume;
        minPitch = _minPitch;
        maxPitch = _maxPitch;
    }
}
