using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Button NextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        NextLevelButton.onClick.AddListener(handleNextLevel);
    }

    private void handleNextLevel()
    {
        Destroy(gameObject);

        GlobalEvents.NextLevelEvent.Invoke();
    }
}
