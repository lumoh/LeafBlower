using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float FadeDuration;
    public float Delay;

    void Start()
    {
        canvasGroup.DOFade(0f, FadeDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetDelay(Delay);
    }
}
