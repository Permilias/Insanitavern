using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void TransitionToScene(string sceneName)
    {
        DataManager.Save();

        if(!TransitionScreen.Instance)
        {
            Debug.Log("No Transition Screen Available !");
            GameManager.LoadScene(sceneName);
            return;
        }

        StartCoroutine(TransitionToSceneCoroutine(sceneName));
    }

    public void TransitionToSceneNoSave(string sceneName)
    {
        if (!TransitionScreen.Instance)
        {
            Debug.Log("No Transition Screen Available !");
            GameManager.LoadScene(sceneName);
            return;
        }

        StartCoroutine(TransitionToSceneCoroutine(sceneName));
    }

    IEnumerator TransitionToSceneCoroutine(string sceneName)
    {
        TransitionScreen.Instance.Close();
        yield return new WaitForSeconds(TransitionScreen.Instance.closingSpeed);
        GameManager.LoadScene(sceneName);
    }
}
