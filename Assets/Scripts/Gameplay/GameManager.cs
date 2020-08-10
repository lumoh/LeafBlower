using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AnalyticsType AnalyticsType;
    public Level Level;
    public GameObject PlayerObj;
    public bool IsLevelOver;

    [Header("Config Vars")]
    public int MaxLevel = 1;
    public bool CheatMenuEnabled;
    public int TargetFrameRate = 60;
    public int SolverIterations = 5;
    public bool ParticlesEnabled;
    public bool DestroyWhenCollected;
    public bool GoalPointers;

    [Header("Special covid prefab")]
    public GameObject CovidPrefab;

    [Header("IAP Manager")]
    public bool IAPEnabled;
    public IAPManager IAP;

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
        Physics.defaultSolverIterations = SolverIterations;

        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.PLAYER, true);
        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.FENCE, true);
        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.LEAF, true);
        Physics.IgnoreLayerCollision(Layers.LEAF, Layers.EDITOR, true);

        instance = this;

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
    }

    private void OnApplicationQuit()
    {
        Analytics.AppQuit();
    }

    // Start is called before the first frame update
    void Start()
    {
        Analytics.AppStart();
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

        int levelNum = PlayerState.GetLevel();
        string levelName = "Level_" + levelNum;
        if (levelNum > MaxLevel)
        {
            levelName = "Level_Zen";
        }
        Level levelPrefab = Resources.Load<Level>("Levels/" + levelName);
        Level = Instantiate(levelPrefab);
        Level.Num = levelNum;
		Level.transform.position = Vector3.zero;

        GameObject playerPrefab = Resources.Load("Player") as GameObject;
        PlayerObj = Instantiate(playerPrefab) as GameObject;
        PlayerObj.transform.position = Level.PlayerSpawn.position;
        PlayerObj.transform.rotation = Level.PlayerSpawn.rotation;

        IsLevelOver = false;        
        
        GlobalEvents.LevelLoaded.Invoke();

        Level.DoLevelPan();

        MenuManager.RemoveLoadingScreen(()=>
        {
            SoundManager.instance.PlayMusic("game", 1f, false, true);
        });
    }

    public static bool IsZenLevel()
    {
        bool isZen = GameManager.instance != null && GameManager.instance.Level != null && GameManager.instance.Level.IsZen;
        return isZen;
    }

    public static bool AdsEnabled()
    {
        bool enabled = !PlayerState.GetBool(PlayerState.AD_BLOCK);
        return enabled;
    }

    public static void SetAds(bool enabled)
    {
        Debug.Log("Ads are now " + (enabled ? "ON" : "OFF"));
        PlayerState.SetBool(PlayerState.AD_BLOCK, !enabled);
        GlobalEvents.AdsStatusChanged.Invoke();
    }

    private void handleWinLevel()
    {
        if (!IsLevelOver)
        {
            IsLevelOver = true;
            LevelNum = Mathf.Min(LevelNum + 1, MaxLevel + 1);
            MenuManager.PushMenu(MenuManager.WIN);

            PlayerState.WinLevel(Level.Num);
            Analytics.WinLevel(Level.Num, Timer.instance.GetSecondsLeft());            
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

    private void handleStartLevel()
    {
        PlayerState.StartLevel(Level.Num);
        Analytics.StartLevel(Level.Num);        
    }
}
