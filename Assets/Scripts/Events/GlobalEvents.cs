using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEvents
{
    public static UnityEvent LoseLevel = new UnityEvent();
    public static UnityEvent RetryLevel = new UnityEvent();
    public static UnityEvent StartLevel = new UnityEvent();
    public static UnityEvent WinLevel = new UnityEvent();
}
