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
    public Button NoAdsButton;
    public GameObject ZenModeObj;
    public CanvasGroup NotifCanvasGroup;

    [Header("Tutorial")]
    public Text TutorialText;
    public RectTransform TutorialTextParent;

    [Header("Cheats")]
    public GameObject CheatMenu;
    public TMP_Text AdsText;

    private bool showHudState;
    private bool _playedTutorial;

    private const float PURCHASE_TIMEOUT = 60f;

    // Start is called before the first frame update
    void Start()
    {
        StartText.transform.parent.gameObject.SetActive(false);

        if (NotifCanvasGroup != null)
        {
            NotifCanvasGroup.DOFade(0.5f, FadeDuration).SetLoops(-1, LoopType.Yoyo);
        }

        // Show/Hide cheat menu
        CheatMenu.SetActive(GameManager.instance.CheatMenuEnabled);

        setHaptics();
        setZenMode();
        setNoAds();
        setTutorialText();

        GlobalEvents.AdsStatusChanged.AddListener(setNoAds);
    }

    private void showTapToStart()
    {
        StartText.transform.parent.gameObject.SetActive(true);
        TapToStartButton.onClick.AddListener(handleStartButtonPressed);
        StartText.DOFade(0.5f, FadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void setTutorialText()
    {
        if(TutorialText != null)
        {
            if(GameManager.instance.Level.Num == 1)
            {                
                TutorialText.text = "Blow all the blocks off the platform before time runs out!";
                _playedTutorial = true;
            }
            else if (GameManager.instance.Level.Num == 8)
            {
                TutorialText.text = "Careful, you can fall off the platforms!";
                _playedTutorial = true;
            }
            else if (GameManager.instance.Level.Num == 11)
            {
                TutorialText.text = "Heads up! Blue platforms will move!";
                _playedTutorial = true;
            }
            else
            {
                _playedTutorial = false;
            }
        }

        if (!_playedTutorial)
        {
            TutorialTextParent.gameObject.SetActive(false);
            showTapToStart();
        }
        else
        {
            TutorialTextParent.anchoredPosition = new Vector2(0, 150);
            TutorialTextParent.gameObject.SetActive(true);
            TutorialTextParent.DOAnchorPos(new Vector2(0, -575), 0.5f).SetEase(Ease.OutBack);
            TutorialTextParent.DOAnchorPos(new Vector2(0, 150), 0.5f).SetEase(Ease.InBack).SetDelay(2f);
            StartCoroutine(showTapCoroutine());
        }
    }

    IEnumerator showTapCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        showTapToStart();
    }

    private void setZenMode()
    {
        if(ZenModeObj != null)
        {
            ZenModeObj.SetActive(GameManager.IsZenLevel());
        }
    }

    private void handleStartButtonPressed()
    {
        MenuManager.PopMenu(gameObject);

        Taptic.Heavy();
        GlobalEvents.StartLevel.Invoke();
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

    public void OnSettings()
    {
        gameObject.SetActive(false);
        MenuManager.PushMenu(MenuManager.SETTINGS);
        GlobalEvents.MenuPopped.AddListener(handleSettingsClosed);
        SoundManager.Button();
    }

    private void handleSettingsClosed(string menuName)
    {
        if(menuName == MenuManager.SETTINGS)
        {
            gameObject.SetActive(true);
            GlobalEvents.MenuPopped.RemoveListener(handleSettingsClosed);
        }
    }

    private void setNoAds()
    {
        NoAdsButton.gameObject.SetActive(GameManager.instance.IAPEnabled && GameManager.AdsEnabled());
        setAdsText();
    }

    public void OnPurchaseNoAds()
    {
        GlobalEvents.PurchaseComplete.AddListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.AddListener(handlePurchaseFailed);
        GlobalEvents.StartPurchase.AddListener(handleStartPurchase);

        MenuManager.ShowLoadingScreen();

        GameManager.instance.IAP.BuyNoAds();
    }

    private void handleStartPurchase()
    {
        SoundManager.Button();
        GlobalEvents.StartPurchase.RemoveListener(handleStartPurchase);
        StartCoroutine(purchaseTimeout());
    }

    private IEnumerator purchaseTimeout()
    {
        yield return new WaitForSeconds(PURCHASE_TIMEOUT);

        handlePurchaseFailed();
    }

    private void handlePurchaseSuccess()
    {
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);
        GlobalEvents.StartPurchase.RemoveListener(handleStartPurchase);
        StopCoroutine(purchaseTimeout());

        MenuManager.RemoveLoadingScreen();
        MenuManager.PushMenu(MenuManager.SUCCESS);
    }

    private void handlePurchaseFailed()
    {
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);
        GlobalEvents.StartPurchase.RemoveListener(handleStartPurchase);
        StopCoroutine(purchaseTimeout());

        MenuManager.RemoveLoadingScreen();
        MenuManager.PushMenu(MenuManager.FAILED);
    }

    private void OnDestroy()
    {
        GlobalEvents.AdsStatusChanged.RemoveListener(setNoAds);
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);
        GlobalEvents.StartPurchase.RemoveListener(handleStartPurchase);
        StopCoroutine(purchaseTimeout());
    }

    // ===== CHEATS SECTION ===== //

    public void CHEAT_ResetData()
    {
        MenuManager.PopMenu(gameObject);

        GameManager.instance.LevelNum = 1;
        GameManager.instance.LoadLevelAndPlayer();       
    }

    public void CHEAT_NextLevel()
    {
        MenuManager.PopMenu(gameObject);

        GameManager.instance.LevelNum = Mathf.Min(GameManager.instance.LevelNum + 1, GameManager.instance.MaxLevel + 1);
        GameManager.instance.LoadLevelAndPlayer();        
    }

    public void CHEAT_PreviousLevel()
    {
        MenuManager.PopMenu(gameObject);

        GameManager.instance.LevelNum = Mathf.Max(GameManager.instance.LevelNum - 1, 1);
        GameManager.instance.LoadLevelAndPlayer();        
    }

    public void CHEAT_LastLevel()
    {
        MenuManager.PopMenu(gameObject);

        GameManager.instance.LevelNum = GameManager.instance.MaxLevel;
        GameManager.instance.LoadLevelAndPlayer();
    }

    public void CHEAT_LoadZen()
    {
        MenuManager.PopMenu(gameObject);

        GameManager.instance.LevelNum = GameManager.instance.MaxLevel + 1;
        GameManager.instance.LoadLevelAndPlayer();
    }

    public void CHEAT_ToggleAds()
    {
        bool toggle = !GameManager.AdsEnabled();
        GameManager.SetAds(toggle);
    }

    public void CHEAT_ToggleUI()
    {
        float alpha = showHudState ? 1f : 0f;
        HUD.instance.CanvasGroup.alpha = alpha;
        MobileControlsUI.instance.CanvasGroup.alpha = alpha;
        showHudState = !showHudState;
    }

    private void setAdsText()
    {
        if(AdsText != null)
        {
            AdsText.text = GameManager.AdsEnabled() ? "Turn Ads Off" : "Turn Ads On";
        }
    }
}
