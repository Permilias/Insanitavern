using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIdler : MonoBehaviour
{
    RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        basePos = rt.anchoredPosition;
    }

    Vector2 basePos;
    Vector2 targetPos;
    Vector2 increment;
    float delay;
    private void Update()
    {
        delay -= Time.deltaTime;
        if(delay <= 0f)
        {
            delay = Random.Range(2f, 4f);
            targetPos = basePos + (Random.insideUnitCircle * 13);
            increment = (targetPos - rt.anchoredPosition) / delay;
        }

        rt.anchoredPosition += increment * Time.deltaTime;
    }
}
