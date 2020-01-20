using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AppCenter.Unity.Analytics;

/// <summary>
/// Analytics tracking provided by Microsoft App Center
/// </summary>
public class AppCenterAnalytics : IAnalytics
{
    public void StartLevel(int level)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));

        Analytics.TrackEvent(Evt.START_LEVEL, data);
    }

    public void WinLevel(int level, float timeLeft)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.TIME_LEFT, timeLeft.ToString("F"));
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));

        Analytics.TrackEvent(Evt.WIN_LEVEL, data);
    }

    public void LoseLevel(int level, string loseType)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.LOSE_TYPE, loseType);
        data.Add(EvtParam.LEAVES_LEFT, HUD.instance.GetLeavesRemaining().ToString());
        data.Add(EvtParam.TIME_LEFT, Timer.instance.GetSecondsLeft().ToString("F"));
        data.Add(EvtParam.RECORD, JsonUtility.ToJson(PlayerState.GetLevelRecord(level)));

        Analytics.TrackEvent(Evt.LOSE_LEVEL, data);
    }

    public void AppStart()
    {
        Analytics.TrackEvent(Evt.APP_START);
    }

    public void AppQuit()
    {
        Analytics.TrackEvent(Evt.APP_QUIT);
    }
}
