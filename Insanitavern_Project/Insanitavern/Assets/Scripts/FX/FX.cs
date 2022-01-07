using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    [HideInInspector]
    public float duration;
    float count;

    private void Update()
    {
        if (count <= 0f) return;

        count -= Time.deltaTime;
        if(count < 0f)
        {
            count = 0f;
            Repool();
        }
    }

    public virtual void Play()
    {
        if(pool.data.randomizeSize)
        {
            float scale = Random.Range(pool.data.minMaxSize.x, pool.data.minMaxSize.y);
            transform.localScale = Vector3.one * scale;
        }
        if(pool.data.randomizeRotation)
        {
            float x = Random.Range(0, 360);
            float y = Random.Range(0, 360);
            float z = Random.Range(0, 360);

            transform.eulerAngles = new Vector3(x, y, z);
        }
        duration = pool.data.duration;
        count = duration;
    }

    [HideInInspector]
    public FXPool pool;
    void Repool()
    {
        pool.Pool(this);
    }
}
