using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float FadeDuration = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public void Init(TweenCallback callback)
    {
        if (callback != null)
        {
            canvasGroup.DOFade(1f, FadeDuration).OnComplete(callback);
        }
        else
        {
            canvasGroup.DOFade(1f, FadeDuration);
        }
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(0f, FadeDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
