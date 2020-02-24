using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalsLeft : MonoBehaviour
{
    public Text GoalsText;
    public GameObject Infinity;

    private int _numTotal;
    private int _numCollected;

    private void Awake()
    {
        GlobalEvents.LevelLoaded.AddListener(handleLevelLoaded);
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);
    }

    private void handleLevelLoaded()
    {
        if(GameManager.IsZenLevel())
        {
            Infinity.SetActive(true);
            GoalsText.gameObject.SetActive(false);
        }
        else
        {
            _numTotal = GameManager.instance.Level.NumLeaves;
            _numCollected = 0;
            setText();
            Infinity.SetActive(false);
            GoalsText.gameObject.SetActive(true);
        }
    }

    private void handleLeafCollected()
    {
        _numCollected++;
        setText();
    }

    private void setText()
    {
        GoalsText.text = _numCollected + "/" + _numTotal;
    }
}
