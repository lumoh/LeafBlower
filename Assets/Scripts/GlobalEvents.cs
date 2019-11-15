using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEvents
{
    public static UnityEvent LoseGame = new UnityEvent();
    public static UnityEvent RetryGame = new UnityEvent();
    public static UnityEvent StartRound = new UnityEvent();
    public static UnityEvent WinRound = new UnityEvent();
}
