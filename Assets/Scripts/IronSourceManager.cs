using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class IronSourceManager : MonoBehaviour
{
#if UNITY_ANDROID
    private const string YOUR_APP_KEY = "b765fea5";
#elif UNITY_IOS
    private const string YOUR_APP_KEY = "aa3b78f5";
#endif

    private bool _interstitialFailedLoad;
    private bool _rewardedVideoAvailability;
    private bool _ironSourceInit;

    // Start is called before the first frame update
    void Start()
    {
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);
        IronSource.Agent.validateIntegration();

        /*
        IronSource.Agent.init(
            YOUR_APP_KEY,
            IronSourceAdUnits.BANNER,
            IronSourceAdUnits.INTERSTITIAL,
            IronSourceAdUnits.REWARDED_VIDEO
        );
        */

        IronSource.Agent.init(YOUR_APP_KEY);

        // Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.LevelLoaded.AddListener(handleLevelLoaded);
        GlobalEvents.WinLevel.AddListener(hideBanner);
        GlobalEvents.LoseLevel.AddListener(hideBanner);
        GlobalEvents.RetryLevelEvent.AddListener(showRewardedVideo);
        GlobalEvents.NextLevelEvent.AddListener(showInterstitial);

        StartCoroutine(loadBannerAndInterstitial());
    }

    IEnumerator loadBannerAndInterstitial()
    {
        yield return new WaitForSeconds(3f);

        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        _ironSourceInit = true;
    }

    void handleStartLevel()
    {
        if (_ironSourceInit)
        {
            if (GameManager.instance.AdsEnabled)
            {
                IronSource.Agent.displayBanner();
            }
            else
            {
                IronSource.Agent.hideBanner();
            }
        }
    }

    void handleLevelLoaded()
    {
        _interstitialFailedLoad = false;

        if (GameManager.instance.AdsEnabled && _ironSourceInit)
        {
            IronSource.Agent.loadInterstitial();
        }
    }

    void hideBanner()
    {
        if (_ironSourceInit)
        {
            IronSource.Agent.hideBanner();
        }
    }

    bool rollForAd()
    {
        bool getAd = Random.Range(0, 5) < 3;
        return getAd;
    }

    void showRewardedVideo()
    {
        MenuManager.ShowLoadingScreen(() =>
        {
            if (GameManager.instance.AdsEnabled && _ironSourceInit)
            {
                if (_rewardedVideoAvailability && rollForAd())
                {
                    IronSource.Agent.showRewardedVideo();
                }
                else
                {
                    showInterstitial();
                }
            }
            else
            {
                GameManager.instance.LoadLevelAndPlayer();
            }
        });
    }

    void showInterstitial()
    {
        MenuManager.ShowLoadingScreen(() =>
        {
            if (rollForAd() && _ironSourceInit && GameManager.instance.AdsEnabled && IronSource.Agent.isInterstitialReady() && !_interstitialFailedLoad)
            {
                IronSource.Agent.showInterstitial();
            }
            else
            {
                GameManager.instance.LoadLevelAndPlayer();
            }
        });
    }

    private void OnApplicationPause(bool pause)
    {
        if (_ironSourceInit)
        {
            IronSource.Agent.onApplicationPause(pause);
        }
    }

    #region Banner ads
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
    #endregion

    #region Interstitial ads
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
        if(FB.IsInitialized)
        {
            FB.LogAppEvent(FacebookManager.INTERSTITIAL_COMPLETE_EVENT);
        }
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
    #endregion

    #region Rewarded Video
    //Invoked when the RewardedVideo ad view has opened.
    //Your Activity will lose focus. Please avoid performing heavy 
    //tasks till the video ad will be closed.
    void RewardedVideoAdOpenedEvent()
    {
    }
    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    void RewardedVideoAdClosedEvent()
    {
        //MenuManager.PushMenu(MenuManager.GAME_OVER);
    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available. 
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //Change the in-app 'Traffic Driver' state according to availability.
        _rewardedVideoAvailability = available;
    }
    //  Note: the events below are not available for all supported rewarded video 
    //   ad networks. Check which events are available per ad network you choose 
    //   to include in your build.
    //   We recommend only using events which register to ALL ad networks you 
    //   include in your build.
    //Invoked when the video ad starts playing.
    void RewardedVideoAdStartedEvent()
    {
    }
    //Invoked when the video ad finishes playing.
    void RewardedVideoAdEndedEvent()
    {
    }
    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
    //
    //@param - placement - placement object which contains the reward data
    //
    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        if (FB.IsInitialized)
        {
            FB.LogAppEvent(FacebookManager.REWARDED_VIDEO_COMPLETE_EVENT);
        }
        GameManager.instance.LoadLevelAndPlayer();
    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        GameManager.instance.LoadLevelAndPlayer();
    }
    #endregion
}
