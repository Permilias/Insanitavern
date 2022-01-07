using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static bool init;
    public static void Initialize()
    {
        init = true;
    }

    public static void LoadInit()
    {
        SceneManager.LoadScene("Init");
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static bool willReset;
    public static void ResetGame()
    {
        willReset = true;
        LoadInit();
    }
}
