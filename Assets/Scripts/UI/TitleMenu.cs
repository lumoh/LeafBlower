using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float FadeDuration;
    public float Delay;
    public Text VersionText;

    void Start()
    {
        canvasGroup.DOFade(0f, FadeDuration).OnComplete(() =>
        {
            MenuManager.PopMenu(gameObject);
            SoundManager.instance.PlayMusic("game");
            GlobalEvents.LevelShown.Invoke();
        }).SetDelay(Delay);

        if (VersionText != null)
        {
            VersionText.text = "v" + Application.version;
        }
    }
}
