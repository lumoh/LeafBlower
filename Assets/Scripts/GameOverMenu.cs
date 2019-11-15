using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button RetryButton;

    // Start is called before the first frame update
    public GameOverMenu()
    {
        RetryButton.onClick.AddListener(handleRetryPressed);
        GlobalEvents.LoseGame.AddListener(handleLoseGame);
    }

    private void handleRetryPressed()
    {
        gameObject.SetActive(false);
        GlobalEvents.RetryGame.Invoke();
    }

    private void handleLoseGame()
    {
        gameObject.SetActive(true);
    }
}
