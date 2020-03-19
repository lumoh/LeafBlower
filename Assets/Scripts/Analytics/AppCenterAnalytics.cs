using System.Collections.Generic;
using UnityEngine;
using Microsoft.AppCenter.Unity.Analytics;
using Facebook.Unity;

/// <summary>
/// Analytics tracking provided by Microsoft App Center.
/// Now also logging facebook
/// </summary>
public class AppCenterAnalytics : IAnalytics
{
    public void StartLevel(int level)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));
        Analytics.TrackEvent(Evt.START_LEVEL, data);

        Dictionary<string, object> fbData = new Dictionary<string, object>();
        fbData.Add(EvtParam.LEVEL, level.ToString());
        fbData.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));

        FB.LogAppEvent(Evt.START_LEVEL, null, fbData);
    }

    public void WinLevel(int level, float timeLeft)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.TIME_LEFT, timeLeft.ToString("F"));
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));
        Analytics.TrackEvent(Evt.WIN_LEVEL, data);

        Dictionary<string, object> fbData = new Dictionary<string, object>();
        fbData.Add(EvtParam.LEVEL, level.ToString());
        fbData.Add(EvtParam.TIME_LEFT, timeLeft.ToString("F"));
        fbData.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));
        FB.LogAppEvent(Evt.WIN_LEVEL, null, fbData);

#if UNITY_ANDROID
        if(level == 1)
        {
            Social.ReportProgress("CgkIm-bagYAIEAIQAA", 10, (bool success)=>
            {

            });
        }
#endif
    }

    public void LoseLevel(int level, string loseType)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.LOSE_TYPE, loseType);
        data.Add(EvtParam.GOALS_LEFT, HUD.instance.GetLeavesRemaining().ToString());
        data.Add(EvtParam.TIME_LEFT, Timer.instance.GetSecondsLeft().ToString("F"));
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));
        Analytics.TrackEvent(Evt.LOSE_LEVEL, data);

        Dictionary<string, object> fbData = new Dictionary<string, object>();
        fbData.Add(EvtParam.LEVEL, level.ToString());
        fbData.Add(EvtParam.LOSE_TYPE, loseType);
        fbData.Add(EvtParam.GOALS_LEFT, HUD.instance.GetLeavesRemaining().ToString());
        fbData.Add(EvtParam.TIME_LEFT, Timer.instance.GetSecondsLeft().ToString("F"));
        fbData.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));
        FB.LogAppEvent(Evt.LOSE_LEVEL, null, fbData);
    }

    public void AppStart()
    {
        Analytics.TrackEvent(Evt.APP_START);        
    }

    public void AppQuit()
    {
        Analytics.TrackEvent(Evt.APP_QUIT);

        FB.LogAppEvent(Evt.APP_QUIT);
    }
}
