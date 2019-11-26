using System.Collections;
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

	// game specific
	public static UnityEvent LeafCollected = new UnityEvent();
}
