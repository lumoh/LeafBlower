using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TargetFrameRate = 60;
    public int LevelNum;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }

    private void handleWinLevel()
    {
        LevelNum++;

        MenuManager.PushMenu("WinMenu");
    }
}
