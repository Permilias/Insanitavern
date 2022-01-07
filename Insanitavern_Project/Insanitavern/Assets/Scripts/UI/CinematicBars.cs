using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CinematicBars : MonoBehaviour
{
    public static CinematicBars Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float outDist;
    public float moveSpeed;
    public RectTransform topRT, botRT;

    public void Depop()
    {
        topRT.gameObject.SetActive(false);
        botRT.gameObject.SetActive(false);
        topRT.anchoredPosition = new Vector2(0, outDist);
        botRT.anchoredPosition = new Vector2(0, -outDist);
    }

    public void Pop()
    {
        topRT.gameObject.SetActive(true);
        botRT.gameObject.SetActive(true);
        topRT.anchoredPosition = new Vector2(0, 0);
        botRT.anchoredPosition = new Vector2(0, 0);
    }

    public void Display()
    {
        topRT.gameObject.SetActive(true);
        botRT.gameObject.SetActive(true);
        topRT.DOAnchorPos(new Vector2(0, 0), moveSpeed);
        botRT.DOAnchorPos(new Vector2(0, 0), moveSpeed);
    }

    public void Hide()
    {
        topRT.DOAnchorPos(new Vector2(0, outDist), moveSpeed).OnComplete(() =>
        {
            topRT.gameObject.SetActive(false);
            botRT.gameObject.SetActive(false);
        });
        botRT.DOAnchorPos(new Vector2(0, -outDist), moveSpeed);
    }
}
