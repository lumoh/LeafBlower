using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text RoundText;
    public Text ScoreText;

    private int score = 0;
    private int maxScore = 0;
    private int roundNum = 0;

    // Start is called before the first frame update
    void Awake()
    {
        LeafSpawner.LeafCollectedEvent.AddListener(handleLeafCollected);
        LeafSpawner.LeavesSpawned.AddListener(handleLeavesSpawned);
    }

    void handleLeafCollected()
    {
        score++;
        ScoreText.text = score + "/" + maxScore;
    }

    void handleLeavesSpawned(int leavesSpawed)
    {
        score = 0;
        maxScore = leavesSpawed;
        roundNum++;
        RoundText.text = "Round " + roundNum;
        ScoreText.text = score + "/" + maxScore;
    }
}
