using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitionScreen : MonoBehaviour
{
    public static TransitionScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject graphics;
    public RectTransform mask;

    public bool startsOpen;

    public float openingSpeed;
    public float closingSpeed;

    private void Start()
    {
        if(startsOpen)
        {
            OpenNoDelay();
        }
        else
        {
            CloseNoDelay();
            Open();
        }
    }

    public void OpenNoDelay()
    {

        mask.localScale = Vector3.one;
        graphics.SetActive(false);
    }

    public void CloseNoDelay()
    {
        mask.gameObject.SetActive(true);
        graphics.SetActive(true);
        mask.localScale = Vector3.zero;
    }

    public void Open()
    {
        mask.localScale = Vector3.zero;
        mask.gameObject.SetActive(true);
        graphics.SetActive(true);
        mask.DOScale(Vector3.one* 1.6f, openingSpeed).OnComplete(() =>
        {
            graphics.SetActive(false);
        });
    }

    public void Close()
    {
        mask.localScale = Vector3.one * 1.6f;
        graphics.SetActive(true);
        mask.gameObject.SetActive(true);
        mask.DOScale(Vector3.zero, closingSpeed).OnComplete(() =>
        {
            mask.gameObject.SetActive(false);
        });
    }
}
