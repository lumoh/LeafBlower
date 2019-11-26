﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TargetFrameRate = 60;
    public int LevelNum = 1;
    public int MaxLevel = 1;
    public AnalyticsType AnalyticsType;

    public Level Level;
    public GameObject PlayerObj;

    private IAnalytics _analytics;
    public IAnalytics Analytics
    {
        get
        {
            if(_analytics == null)
            {
                if (AnalyticsType == AnalyticsType.APP_CENTER)
                {
                    _analytics = new AppCenterAnalytics();
                }
                else
                {
                    _analytics = new NullAnalytics();
                }
            }

            return _analytics;
        }
    }

    public static GameManager instance;

    private void Awake()
    {
        Application.targetFrameRate = TargetFrameRate;

        instance = this;

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
    }

    // Start is called before the first frame update
    void Start()
    {        
        LoadLevelAndPlayer();
    }

    public void LoadLevelAndPlayer()
    {
        if(Level != null)
        {
            Destroy(Level.gameObject);
        }

        if(PlayerObj != null)
        {
            Destroy(PlayerObj);
        }

        string levelName = "Level" + LevelNum;
        Level levelPrefab = Resources.Load<Level>("Levels/" + levelName);
        Level = Instantiate(levelPrefab);
		Level.transform.position = Vector3.zero;

        GameObject playerPrefab = Resources.Load("Player") as GameObject;
        PlayerObj = Instantiate(playerPrefab) as GameObject;
        PlayerObj.transform.position = Level.PlayerSpawn.position;
        PlayerObj.transform.rotation = Level.PlayerSpawn.rotation;

        MenuManager.PushMenu(MenuManager.HOME);
        GlobalEvents.LevelLoaded.Invoke();
    }

    private void handleWinLevel()
    {
        LevelNum++;
        LevelNum = Mathf.Min(LevelNum, MaxLevel);
        MenuManager.PushMenu(MenuManager.WIN);
    }

    private void handleLoseLevel()
    {
        MenuManager.PushMenu(MenuManager.GAME_OVER);
    }
}
