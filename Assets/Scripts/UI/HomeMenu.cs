using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Home menu - starts the game
/// </summary>
public class HomeMenu : MonoBehaviour
{
    public Button TapToStartButton;
    public Text StartText;

    public float FadeDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        TapToStartButton.onClick.AddListener(handleStartButtonPressed);

        StartText.DOFade(0f, FadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void handleStartButtonPressed()
    {
        GlobalEvents.StartLevel.Invoke();
        Destroy(gameObject);
    }
}
