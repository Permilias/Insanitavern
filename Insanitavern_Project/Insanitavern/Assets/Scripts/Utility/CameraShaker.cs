using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool shaking;
    private void Update()
    {
        if (shaking)
            ShakeUpdate();
        else transform.localPosition = Vector3.zero;
    }

    public float shakeStrength, delay;
    float shakeTimer;
    Vector2 shakePos;
    Vector2 shakeIncrement;

    void ShakeUpdate()
    {
        if (shakeStrength <= 0f) return;

        shakeTimer -= Time.deltaTime;
        if(shakeTimer <= 0f)
        {
            shakeTimer = delay;
            shakePos = Random.insideUnitCircle * shakeStrength;
            shakePos = shakePos - new Vector2(transform.localPosition.x, transform.localPosition.z);
            shakeIncrement = shakePos / delay;
        }
        else
        {
            transform.localPosition += (new Vector3(shakeIncrement.x, 0f, shakeIncrement.y)*  Time.deltaTime);
        }
    }

    public void Shake(float strength, float duration)
    {
        if (hardShaking) return;

        hardShaking = true;
        tempStrength = shakeStrength;
        shakeStrength = strength;
        shakeDuration = duration;
        StartCoroutine(ShakeCoroutine());
    }

    bool hardShaking;
    float shakeDuration;
    float tempStrength;
    IEnumerator ShakeCoroutine()
    {
        yield return new WaitForSeconds(shakeDuration);
        shakeStrength = tempStrength;
        hardShaking = false;
    }
}
