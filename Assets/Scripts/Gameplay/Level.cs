using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int Num;
    public Transform PlayerSpawn;
    public List<GameObject> Platforms;
    public List<LeafCube> LeafCubes;
    public int NumLeaves;
    public int Seconds = 30;

    private int _leavesLeft;

    private void Awake()
    {
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);

        foreach (var leafCube in LeafCubes)
        {
            NumLeaves += leafCube.GetNumLeaves();
        }

        _leavesLeft = NumLeaves;
    }

    private void handleLeafCollected()
    {
        _leavesLeft--;
        if(_leavesLeft == 0)
        {
            GlobalEvents.WinLevel.Invoke();
        }
    }
}
