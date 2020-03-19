using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
    public const string FIRST_LEVEL_WIN_EVENT = "first_level_win";
    public const string LAST_LEVEL_WIN_EVENT = "last_level_win";
    public const string INTERSTITIAL_COMPLETE_EVENT = "interstitial_complete";
    public const string REWARDED_VIDEO_COMPLETE_EVENT = "rewarded_video_complete";

    // Start is called before the first frame update
    void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
                FB.LogAppEvent(Evt.APP_START);
            });
        }

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
    }

    private void handleWinLevel()
    {
        if (GameManager.instance != null && GameManager.instance.Level != null)
        {
            if (GameManager.instance.Level.Num == 1)
            {
                FB.LogAppEvent(FIRST_LEVEL_WIN_EVENT);
            }
            else if(GameManager.instance.Level.Num == GameManager.instance.MaxLevel)
            {
                FB.LogAppEvent(LAST_LEVEL_WIN_EVENT);
            }
        }
    }

    private void OnDestroy()
    {
        GlobalEvents.WinLevel.RemoveListener(handleWinLevel);
    }
}
