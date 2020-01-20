using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnalytics
{
    void AppStart();
    void AppQuit();
    void StartLevel(int level);
    void LoseLevel(int level, string loseType);
    void WinLevel(int level, float timeLeft);
}
