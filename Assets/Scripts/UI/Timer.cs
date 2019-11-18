using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimeLeftText;
    public int TotalSeconds = 30;

    private float _secondsLeft;
    private bool _isRunning;

    void Awake()
    {
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.RetryLevel.AddListener(handleRetryLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.WinLevel.AddListener(handleWinLevel);

        _secondsLeft = TotalSeconds;
        TimeLeftText.text = _secondsLeft.ToString("00.0");
    }

    private void handleRetryLevel()
    {
        // TODO: Get time for level
        _secondsLeft = TotalSeconds;
        _isRunning = true;
    }

    public void handleStartLevel()
    {
        _secondsLeft = TotalSeconds;
        _isRunning = true;
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
                GlobalEvents.LoseLevel.Invoke();
            }

            TimeLeftText.text = _secondsLeft.ToString("00.0");
        }
    }
}
