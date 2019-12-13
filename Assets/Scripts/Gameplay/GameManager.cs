using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TargetFrameRate = 60;
    public AnalyticsType AnalyticsType;
    public Level Level;
    public GameObject PlayerObj;
    public bool IsLevelOver;

    [Header("Config Vars")]
    public int MaxLevel = 1;
    public bool IdleAnimEnabled;
    public bool CheatMenuEnabled;

    public int LevelNum
    {
        get
        {
            return PlayerState.GetLevel();
        }
        set
        {
            PlayerState.SetLevel(value);
        }
    }

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
        Debug.Log("GameManager - Awake");
        Application.targetFrameRate = TargetFrameRate;

        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.PLAYER, true);
        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.FENCE, true);
        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.LEAF, true);

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

        string levelName = "Level_" + PlayerState.GetLevel();
        Level levelPrefab = Resources.Load<Level>("Levels/" + levelName);
        Level = Instantiate(levelPrefab);
		Level.transform.position = Vector3.zero;

        GameObject playerPrefab = Resources.Load("Player") as GameObject;
        PlayerObj = Instantiate(playerPrefab) as GameObject;
        PlayerObj.transform.position = Level.PlayerSpawn.position;
        PlayerObj.transform.rotation = Level.PlayerSpawn.rotation;

        IsLevelOver = false;

        MenuManager.PushMenu(MenuManager.HOME);
        GlobalEvents.LevelLoaded.Invoke();
    }

    private void handleWinLevel()
    {
        if (!IsLevelOver)
        {
            IsLevelOver = true;
            LevelNum = Mathf.Min(LevelNum + 1, MaxLevel);
            MenuManager.PushMenu(MenuManager.WIN);
        }
    }

    private void handleLoseLevel()
    {
        if (!IsLevelOver)
        {
            IsLevelOver = true;
            MenuManager.PushMenu(MenuManager.GAME_OVER);
        }
    }
}
