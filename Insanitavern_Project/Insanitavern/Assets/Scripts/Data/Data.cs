using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Data
{
    public bool saved;
    public bool muted;
    public bool fullscreen;
    public float soundVolume;

    public List<SceneData> sceneDatas;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public bool[] validatedItems;
    public int[] itemIndexes;

    public SceneData(string _sceneName, bool[] _validatedItems)
    {
        sceneName = _sceneName;
        validatedItems = _validatedItems;
        itemIndexes = new int[0];
    }
}
