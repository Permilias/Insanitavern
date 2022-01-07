using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Config/Sound", order = 301)]
public class SoundConfig : ScriptableObject
{
    public float baseVolume;
    public int poolingAmount;
    public Sound[] sounds;
}
