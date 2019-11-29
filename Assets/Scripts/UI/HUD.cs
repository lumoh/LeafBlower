using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    public Text LevelNum;
    public Text NextLevelNum;
	public ProgressBar ProgressBar;
	public Image EndFillImage;

    private int _score;
    private int _maxScore;

    // Start is called before the first frame update
    void Awake()
    {
        GlobalEvents.WinLevel.AddListener(handleWinLevel);
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.LevelLoaded.AddListener(handleLevelLoaded);
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);
    }

    void handleLevelLoaded()
    {
        _score = 0;
        _maxScore = GameManager.instance.Level.NumLeaves;

        int levelNum = GameManager.instance.LevelNum;
        int nextLevelNum = levelNum + 1;

        LevelNum.text = levelNum.ToString();
        NextLevelNum.text = nextLevelNum.ToString();

        ProgressBar.specifiedValue = 0;
        EndFillImage.fillAmount = 0f;
    }

    void handleWinLevel()
    {
        if(EndFillImage != null)
        {
            EndFillImage.DOFillAmount(1f, 0.2f);
        }
    }

    void handleStartLevel()
    {
        
    }

    void handleLeafCollected()
    {
        _score++;
        float fillAmount = (float)_score / _maxScore;

        ProgressBar.specifiedValue = fillAmount * 100;
    }

    void handleLeavesSpawned(int leavesSpawed)
    {
        _score = 0;
        _maxScore = leavesSpawed;

        int levelNum = GameManager.instance.LevelNum;
        int nextLevelNum = levelNum + 1;

        LevelNum.text = levelNum.ToString();
        NextLevelNum.text = nextLevelNum.ToString();

        ProgressBar.specifiedValue = 0;
    }
}
