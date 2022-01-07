using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPool : MonoBehaviour
{
    public Queue<FX> pool;
    public FXData data;

    public void Initialize(FXData _data)
    {
        data = _data;
        CreatePool();
    }

    public void CreatePool()
    {
        pool = new Queue<FX>();
        FillPool();
    }

    public void FillPool()
    {
        for(int i = 0; i < data.poolingAmount; i++)
        {
            FX newFX = Instantiate(data.fx, transform).GetComponent<FX>();
            newFX.pool = this;
            Pool(newFX);
        }
    }

    public void Pool(FX fx)
    {
        fx.transform.parent = transform;
        fx.gameObject.SetActive(false);
        pool.Enqueue(fx);
    }

    public FX Depool()
    {
        if(pool.Count < 1)
        {
            FillPool();
        }
        FX depooledFX = pool.Dequeue();
        depooledFX.gameObject.SetActive(true);
        return depooledFX;
    }
}
