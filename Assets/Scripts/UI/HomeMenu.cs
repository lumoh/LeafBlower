using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// Home menu - starts the game
/// </summary>
public class HomeMenu : MonoBehaviour
{
    public Button TapToStartButton;
    public Text StartText;    
    public float FadeDuration;
    public Toggle ToggleHaptics;

    [Header("Cheats")]
    public GameObject CheatMenu;
    public TMP_Text AdsText;

    // Start is called before the first frame update
    void Start()
    {
        TapToStartButton.onClick.AddListener(handleStartButtonPressed);

        StartText.DOFade(0f, FadeDuration).SetLoops(-1, LoopType.Yoyo);

        // Show/Hide cheat menu
        CheatMenu.SetActive(GameManager.instance.CheatMenuEnabled);
        setAdsText();
        setHaptics();
    }

    private void handleStartButtonPressed()
    {
        Taptic.Heavy();
        GlobalEvents.StartLevel.Invoke();
        Destroy(gameObject);
    }

    private void setHaptics()
    {
        if (ToggleHaptics != null)
        {
            ToggleHaptics.isOn = !PlayerState.GetBool(PlayerState.HAPTICS);
        }
    }

    public void OnToggleHaptics()
    {
        if (ToggleHaptics != null)
        {
            Taptic.tapticOn = !ToggleHaptics.isOn;
            PlayerState.SetBool(PlayerState.HAPTICS, Taptic.tapticOn);
            Taptic.Default();
        }
    }

    // ===== CHEATS SECTION ===== //

    public void CHEAT_ResetData()
    {
        GameManager.instance.LevelNum = 1;
        GameManager.instance.LoadLevelAndPlayer();
        Destroy(gameObject);
    }

    public void CHEAT_NextLevel()
    {
        GameManager.instance.LevelNum = Mathf.Min(GameManager.instance.LevelNum + 1, GameManager.instance.MaxLevel);
        GameManager.instance.LoadLevelAndPlayer();
        Destroy(gameObject);
    }

    public void CHEAT_LastLevel()
    {
        GameManager.instance.LevelNum = Mathf.Max(GameManager.instance.LevelNum - 1, 1);
        GameManager.instance.LoadLevelAndPlayer();
        Destroy(gameObject);
    }

    public void CHEAT_ToggleAds()
    {
        GameManager.instance.AdsEnabled = !GameManager.instance.AdsEnabled;
        setAdsText();
    }

    private void setAdsText()
    {
        if(AdsText != null)
        {
            AdsText.text = GameManager.instance.AdsEnabled ? "Turn Ads Off" : "Turn Ads On";
        }
    }
}
