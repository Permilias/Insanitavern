using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public bool paused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PauseUI.Instance.graphics.SetActive(false);
    }

    private void Update()
    {
        if(InputUtility.EscapePressed())
        {
            TogglePause();
        }

        if(paused)
        {
            if(Input.GetKeyUp(KeyCode.R))
            {
                Time.timeScale = 1;
                GameManager.ResetGame();
            }
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        PauseUI.Instance.graphics.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }
}
