using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDisplay : MonoBehaviour
{
    public Transform graphicsParent;
    public GameObject beerObject, damageObject;

    public void DamageFrame()
    {
        StartCoroutine(DamageFrameCoroutine());
    }

    IEnumerator DamageFrameCoroutine()
    {
        damageObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        damageObject.SetActive(false);
    }
}

