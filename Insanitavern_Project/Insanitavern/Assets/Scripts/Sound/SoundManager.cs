using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static SoundConfig config;

    public static Queue<SoundReceptacle> receptaclePool;

    public static float sfxVolume;

    static GameObject receptacleParent;

    public static void Initialize()
    {
        if(DataManager.data.saved)
        {
            sfxVolume = DataManager.data.soundVolume;
        }
        else
        {
            sfxVolume = config.baseVolume;
        }

        receptacleParent = new GameObject("SoundReceptacleParent");
        GameObject.DontDestroyOnLoad(receptacleParent);

        templateReceptacle = new GameObject();
        templateReceptacle.transform.parent = receptacleParent.transform;

        templateReceptacle.AddComponent<SoundReceptacle>();
        templateReceptacle.GetComponent<SoundReceptacle>().source = templateReceptacle.AddComponent<AudioSource>();

        receptaclePool = new Queue<SoundReceptacle>();

        FillReceptaclePool();
    }

    public static Sound GetSoundByName(string soundName)
    {
        for (int i = 0; i < config.sounds.Length; i++)
        {
            if(config.sounds[i].soundName == soundName)
            {
                return config.sounds[i];
            }
        }

        Debug.LogError("No Sound named " + soundName + " !");
        return config.sounds[0];
    }

    static GameObject templateReceptacle;
    public static void FillReceptaclePool()
    {
        for (int i = 0; i < config.poolingAmount; i++)
        {
            SoundReceptacle newReceptacle = GameObject.Instantiate(templateReceptacle, receptacleParent.transform).GetComponent<SoundReceptacle>();
            receptaclePool.Enqueue(newReceptacle);
        }
    }

    public static void PlaySound(string soundName, bool loops = false, bool bypassAudioListener = false)
    {
        Sound _sound = GetSoundByName(soundName);

        if(receptaclePool.Count < 1)
        {
            FillReceptaclePool();
        }
        SoundReceptacle receptacle = receptaclePool.Dequeue();

        receptacle.PlaySound(_sound, loops, bypassAudioListener);
    }

    public static AudioSource PlaySoundReturnSource(string soundName, bool loops = false, bool bypassAudioListener = false)
    {
        Sound _sound = GetSoundByName(soundName);

        if (receptaclePool.Count < 1)
        {
            FillReceptaclePool();
        }
        SoundReceptacle receptacle = receptaclePool.Dequeue();

        return receptacle.PlaySoundReturnSource(_sound, loops, bypassAudioListener);
    }

    public static AudioSource PlaySoundAtPosition(string soundName, Vector3 pos, float spatialBlend = 0.5f, bool loops = false, float maximumDistance = 30f, float minimumDistance = 5f, bool bypassAudioListener = false)
    {
        Sound _sound = GetSoundByName(soundName);

        if (receptaclePool.Count < 1)
        {
            FillReceptaclePool();
        }
        SoundReceptacle receptacle = receptaclePool.Dequeue();
        return receptacle.PlaySoundAtPosition(_sound, loops, true, pos, spatialBlend, maximumDistance, minimumDistance, bypassAudioListener);
    }

    public static void RepoolReceptacle(SoundReceptacle toRepool)
    {
        toRepool.source.enabled = false;
        toRepool.source.Stop();
        receptaclePool.Enqueue(toRepool);
    }
}
