﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button RetryButton;

    // Start is called before the first frame update
    void Start()
    {
        RetryButton.onClick.AddListener(handleRetryPressed);
    }

    private void handleRetryPressed()
    {
        Destroy(gameObject);
        GlobalEvents.RetryLevel.Invoke();
    }

    private void handleLoseGame()
    {
        gameObject.SetActive(true);
    }
}
