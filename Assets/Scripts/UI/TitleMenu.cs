using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float FadeDuration;

    void Start()
    {
        canvasGroup.DOFade(0f, FadeDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
