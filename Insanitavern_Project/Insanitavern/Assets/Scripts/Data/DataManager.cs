using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public static class DataManager
{

    public static string path;

    public static Data data = new Data();


    public static void Reset()
    {
        data = new Data();
        data.saved = false;
    }

    public static void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "data.json");
    }

    public static void Save()
    {
        Debug.Log("Saving !");
        SetData();
        SerializeData();
    }

    public static void SetData()
    {
        data.saved = true;
        data.soundVolume = SoundManager.sfxVolume;

        if (!HasSceneDataForThisScene())
        {
            Debug.Log("No Scene Data");
            CreateSceneData();
        }

        SavedItemManager.RegisterAllValidationForScene();
    }

    public static int CurrentSceneDataIndex()
    {
        for (int i = 0; i < data.sceneDatas.Count; i++)
        {
            if(data.sceneDatas[i].sceneName == SceneManager.GetActiveScene().name)
            {
                return i;
            }
        }

        return 0;
    }

    public static void SerializeData()
    {
        string dataString = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, dataString);
    }

    public static void Load()
    {
        if (File.Exists(path))
        {
            DeserializeData();
        }
        else
        {
            Reset();
        }
    }

    public static void DeserializeData()
    {
        string loadedString = File.ReadAllText(path);
        data = JsonUtility.FromJson<Data>(loadedString);
    }
    public static void CreateSceneDataIfMissing()
    {
        if (!HasSceneDataForThisScene())
        {
            CreateSceneData();
        }
    }

    public static bool HasSceneDataForThisScene()
    {
        bool hasData = false;
        if (data.sceneDatas != null)
        {
            foreach (SceneData sceneData in data.sceneDatas)
            {
                if (sceneData.sceneName == SceneManager.GetActiveScene().name)
                {
                    hasData = true;
                }
            }
        }
        else Debug.Log("No Scene Data List");

        return hasData;
    }
    public static void CreateSceneData()
    {
        if (data.sceneDatas == null)
        {
            data.sceneDatas = new List<SceneData>();
        }
        SceneData newData = new SceneData(SceneManager.GetActiveScene().name, new bool[0]);
        data.sceneDatas.Add(newData);
    }
}
