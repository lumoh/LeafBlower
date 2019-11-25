using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TargetFrameRate = 60;
    public int LevelNum = 1;
    public AnalyticsType AnalyticsType;

    public GameObject LevelObj;
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
        instance = this;

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;

        loadLevelAndPlayer();

        GlobalEvents.LevelLoaded.Invoke();
    }

    private void loadLevelAndPlayer()
    {
        string levelName = "Level" + LevelNum;
        GameObject levelPrefab = Resources.Load("Levels/" + levelName) as GameObject;
        LevelObj = Instantiate(levelPrefab);
        LevelObj.transform.position = Vector3.zero;

        GameObject playerPrefab = Resources.Load("Player") as GameObject;
        PlayerObj = Instantiate(playerPrefab) as GameObject;

        Transform playerSpawn = LevelObj.transform.Find("PlayerSpawn");
        if(playerSpawn != null)
        {
            PlayerObj.transform.position = playerSpawn.position;
        }
        else
        {
            PlayerObj.transform.position = new Vector3(0, 0, 0);
        }
    }

    private void handleWinLevel()
    {
        LevelNum++;
        MenuManager.PushMenu(MenuManager.WIN);
    }
}
