﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEvents
{
    // generic
    public static UnityEvent LoseLevel = new UnityEvent();
    public static UnityEvent StartLevel = new UnityEvent();
    public static UnityEvent WinLevel = new UnityEvent();
    public static UnityEvent LevelLoaded = new UnityEvent();
    public static UnityEvent LevelShown = new UnityEvent();

    public static UnityEvent RetryWithUnlimitedEvent = new UnityEvent();
    public static UnityEvent RetryLevelEvent = new UnityEvent();
    public static UnityEvent NextLevelEvent = new UnityEvent();

    // game specific
    public static UnityEvent LeafCollected = new UnityEvent();
    public static UnityEvent AdsStatusChanged = new UnityEvent();
    public static MyStringEvent MenuPopped = new MyStringEvent();
    public static UnityEvent PurchaseComplete = new UnityEvent();
    public static UnityEvent PurchaseFailed = new UnityEvent();
    public static UnityEvent StartPurchase = new UnityEvent();
    public static LeafCollectedEvent LeafCollectedInfo = new LeafCollectedEvent();
}
