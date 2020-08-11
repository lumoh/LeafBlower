using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button GetUnlimitedTimeButton;
    public Button RetryButton;

    // Start is called before the first frame update
    void Start()
    {
        GetUnlimitedTimeButton.onClick.AddListener(handleGetUnlimited);
        RetryButton.onClick.AddListener(handleRetryPressed);
    }

    private void handleRetryPressed()
    {
        Taptic.Heavy();
        MenuManager.PopMenu(gameObject);
        GlobalEvents.RetryLevelEvent.Invoke();
    }

    private void handleGetUnlimited()
    {
        Taptic.Heavy();
        MenuManager.PopMenu(gameObject);
        GlobalEvents.RetryWithUnlimitedEvent.Invoke();
    }
}
