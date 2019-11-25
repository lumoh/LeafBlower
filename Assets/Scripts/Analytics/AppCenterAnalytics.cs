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

        Analytics.TrackEvent(Evt.START_LEVEL);
    }

    public void WinLevel(int level, float timeLeft)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.TIME_LEFT, timeLeft.ToString("F"));

        Analytics.TrackEvent(Evt.WIN_LEVEL);
    }

    public void LoseLevel(int level, string loseType)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(EvtParam.LEVEL, level.ToString());
        data.Add(EvtParam.LOSE_TYPE, loseType);

        Analytics.TrackEvent(Evt.LOSE_LEVEL, data);
    }
}
