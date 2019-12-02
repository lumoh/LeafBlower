using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
    private const string YOUR_APP_KEY = "aa3b78f5";

    private bool _interstitialFailedLoad;

    void Awake()
    {
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.WinLevel.AddListener(hideBanner);
        GlobalEvents.LoseLevel.AddListener(hideBanner);
        GlobalEvents.RetryLevelEvent.AddListener(showInterstitial);
        GlobalEvents.NextLevelEvent.AddListener(showInterstitial);
    }

    void handleStartLevel()
    {
        _interstitialFailedLoad = false;
        IronSource.Agent.loadInterstitial();
        IronSource.Agent.displayBanner();
    }

    void hideBanner()
    {
        IronSource.Agent.hideBanner();
    }

    void showInterstitial()
    {
        if(!IronSource.Agent.isInterstitialReady() || _interstitialFailedLoad)
        {
            MenuManager.PushMenu(MenuManager.FAKE_AD);
        }
        else
        {
            IronSource.Agent.showInterstitial();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IronSource.Agent.init(
            YOUR_APP_KEY,
            IronSourceAdUnits.BANNER,
            IronSourceAdUnits.INTERSTITIAL
        );

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        IronSource.Agent.validateIntegration();

        // Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
    }

    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }

    // Banner Events
    void BannerAdLoadedEvent()
    {
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }

    //Invoked when the initialization process has failed.
    //@param description - string - contains information about the failure.
    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log(error.getDescription());
        _interstitialFailedLoad = true;
    }

    //Invoked right before the Interstitial screen is about to open.
    void InterstitialAdShowSucceededEvent()
    {
        GameManager.instance.LoadLevelAndPlayer();
    }
    //Invoked when the ad fails to show.
    //@param description - string - contains information about the failure.
    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log(error.getDescription());
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialAdClickedEvent()
    {
    }
    //Invoked when the interstitial ad closed and the user goes back to the application screen.
    void InterstitialAdClosedEvent()
    {
    }
    //Invoked when the Interstitial is Ready to shown after load function is called
    void InterstitialAdReadyEvent()
    {
    }
    void InterstitialAdOpenedEvent()
    {
    }
}
