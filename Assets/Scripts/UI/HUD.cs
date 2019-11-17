using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text LevelNum;
    public Text NextLevelNum;
    public Meter Meter;

    private int _score;
    private int _maxScore;

    // Start is called before the first frame update
    void Awake()
    {
        LeafSpawner.LeafCollectedEvent.AddListener(handleLeafCollected);
        LeafSpawner.LeavesSpawned.AddListener(handleLeavesSpawned);
    }

    void handleLeafCollected()
    {
        _score++;
        float fillAmount = (float)_score / _maxScore;
        Meter.SetFill(fillAmount);
    }

    void handleLeavesSpawned(int leavesSpawed)
    {
        _score = 0;
        _maxScore = leavesSpawed;

        int levelNum = GameManager.instance.LevelNum;
        int nextLevelNum = levelNum + 1;

        LevelNum.text = levelNum.ToString();
        NextLevelNum.text = nextLevelNum.ToString();

        Meter.SetFill(0);
    }
}
