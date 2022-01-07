using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FXManager
{

    public static FXConfig config;

    public static FXPool[] fxPools;

    public static void Initialize()
    {
        GameObject fxPoolParent = new GameObject("FXPoolParent");
        GameObject.DontDestroyOnLoad(fxPoolParent);

        fxPools = new FXPool[config.fxs.Length];
        for(int i = 0; i < config.fxs.Length; i++)
        {
            GameObject newPoolGO = new GameObject("FXPool_" + config.fxs[i].fxName);
            newPoolGO.transform.parent = fxPoolParent.transform;
            FXPool newPool = newPoolGO.AddComponent<FXPool>();
            newPool.Initialize(config.fxs[i]);
            fxPools[i] = newPool;
        }
    }

    public static void PlayFX(string fxName, Vector3 _position)
    {
        bool found = false;
        for(int i = 0; i < fxPools.Length; i++)
        {
            if(fxPools[i].data.fxName == fxName)
            {
                found = true;
                PlayFXFromPool(fxPools[i], _position);
                break;
            }
        }

        if(!found)
        {
            Debug.LogError("ERROR : No FX named " + fxName + " !");
        }
    }

    public static void PlayParentedFX(string fxName, Vector3 _position, Transform _parent)
    {
        bool found = false;
        for (int i = 0; i < fxPools.Length; i++)
        {
            if (fxPools[i].data.fxName == fxName)
            {
                found = true;
                PlayFXFromPool(fxPools[i], _position, _parent);
                break;
            }
        }

        if (!found)
        {
            Debug.LogError("ERROR : No FX named " + fxName + " !");
        }
    }

    public static void PlayTextMessage(Transform _transform, Color color, string text, float height)
    {
        bool found = false;
        for (int i = 0; i < fxPools.Length; i++)
        {
            if (fxPools[i].data.fxName == "TextMessage")
            {
                found = true;
                PlayFXFromPool(fxPools[i], _transform, true, color, text, height);
                break;
            }
        }

        if (!found)
        {
            Debug.LogError("ERROR : No Text Message !");
        }
    }

    public static void PlayFXFromPool(FXPool pool,Vector3 _position)
    {
        FX playedFX = pool.Depool();
        playedFX.transform.position = _position;
        playedFX.Play();
    }

    public static void PlayFXFromPool(FXPool pool, Vector3 _position, Transform _parent)
    {
        FX playedFX = pool.Depool();
        playedFX.transform.position = _position;
        playedFX.Play();
        playedFX.transform.parent = _parent;
    }

    public static void PlayFXFromPool(FXPool pool, Transform _transform, bool textMessage, Color color, string text, float height)
    {
        FX playedFX = pool.Depool();
        playedFX.transform.parent = _transform;
        playedFX.transform.localPosition = Vector3.zero;

        if(textMessage)
        {
            TextMessage message = (TextMessage)playedFX;
            message.SetTextAndPlay(text, color, height);
        }
        else
        {
            playedFX.Play();
        }

    }
}
