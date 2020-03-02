using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class GooglePlayServicesManager : MonoBehaviour
{
#if UNITY_ANDROID
    void Start()
    {
        PlayGamesPlatform.Activate();
    }
#endif
}
