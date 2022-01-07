using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName =  "FXConfig", menuName = "Config/FX", order = 300)]
public class FXConfig : ScriptableObject
{
    public FXData[] fxs;

    public float textMessageEnterSpeed;
    public Ease textMessageEnterEase;
    public float textMessageDuration;
    public float textMessageExitSpeed;
    public Ease textMessageExitEase;
    public float textMessageExitHeight;
}
