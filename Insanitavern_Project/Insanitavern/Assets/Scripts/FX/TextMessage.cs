using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextMessage : FX
{
    float targetHeight;
    public TextMeshPro textMesh;
    public void SetTextAndPlay(string _text, Color _color, float height)
    {
        duration = FXManager.config.textMessageEnterSpeed + FXManager.config.textMessageDuration + FXManager.config.textMessageExitSpeed;
        targetHeight = height;
        textMesh.text = _text;
        textMesh.color = new Color(_color.r, _color.g, _color.b, 0f);
        Play();
    }

    public override void Play()
    {
        base.Play();
        textMesh.transform.localPosition = Vector3.zero;

        textMesh.DOFade(1f, FXManager.config.textMessageEnterSpeed);
        textMesh.transform.DOLocalMove(Vector3.zero + new Vector3(0, targetHeight, 0), FXManager.config.textMessageEnterSpeed).SetEase(FXManager.config.textMessageEnterEase).OnComplete(() =>
        {
            textMesh.transform.DOLocalMove(textMesh.transform.localPosition, FXManager.config.textMessageDuration).OnComplete(() =>
            {
                textMesh.DOFade(0f, FXManager.config.textMessageExitSpeed);
                textMesh.transform.DOLocalMove(textMesh.transform.localPosition + new Vector3(0, FXManager.config.textMessageExitHeight, 0), FXManager.config.textMessageExitSpeed).SetEase(FXManager.config.textMessageExitEase);
            });
        });

    }
}
