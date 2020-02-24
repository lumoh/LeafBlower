using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimeLeftText;
    public int TotalSeconds = 30;
    public GameObject Infinity;

    private float _secondsLeft;
    private bool _isRunning;

    public static Timer instance;

    void Awake()
    {
        instance = this;

        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.WinLevel.AddListener(handleWinLevel);
		GlobalEvents.LevelLoaded.AddListener(handleLevelLoaded);

		_secondsLeft = TotalSeconds;
        setText();
    }

    private void setText()
    {
        if(GameManager.IsZenLevel())
        {
            gameObject.SetActive(false);
            TimeLeftText.gameObject.SetActive(false);
            Infinity.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            TimeLeftText.text = _secondsLeft.ToString("00.0");
            TimeLeftText.gameObject.SetActive(true);
            Infinity.SetActive(false);
        }        
    }

    public void handleLevelLoaded()
    {
        TotalSeconds = GameManager.instance.Level.Seconds;
        _secondsLeft = TotalSeconds;
        setText();
    }

    public void handleStartLevel()
    {
        if (!GameManager.IsZenLevel())
        {
            _isRunning = true;
        }
    }

    private void handleLoseLevel()
    {
        _isRunning = false;
    }

    private void handleWinLevel()
    {
        _isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            _secondsLeft -= Time.deltaTime;
            if (_secondsLeft < 0)
            {
                _secondsLeft = 0f;
                _isRunning = false;
                GameManager.instance.Analytics.LoseLevel(GameManager.instance.Level.Num, "Time");
                GlobalEvents.LoseLevel.Invoke();
            }

            TimeLeftText.text = _secondsLeft.ToString("00.0");
        }
    }

    public float GetSecondsLeft()
    {
        return _secondsLeft;
    }
}
