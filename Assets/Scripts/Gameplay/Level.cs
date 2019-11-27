using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public int Num;
    public Transform PlayerSpawn;
	public Transform PlatformsParent;
	public Transform LeavesParent;

    public int NumLeaves;
    public int Seconds = 30;

    private List<Transform> _platforms;
    private List<LeafCube> _leafCubes;
    private int _leavesLeft;

    private void Awake()
    {
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);

        string[] split = gameObject.name.Split('_');
        int.TryParse(split[1], out Num);

        fetchPlatforms();
        fetchLeaves();

        foreach (var leafCube in _leafCubes)
        {
            NumLeaves += leafCube.GetNumLeaves();
        }

        _leavesLeft = NumLeaves;
    }

    private void fetchPlatforms()
	{
        _platforms = new List<Transform>();
        for (int i = 0; i < PlatformsParent.childCount; i++)
        {
            _platforms.Add(PlatformsParent.GetChild(i));
        }
	}

    private void fetchLeaves()
	{
        LeafCube[] cubes = LeavesParent.GetComponentsInChildren<LeafCube>();
        _leafCubes = new List<LeafCube>(cubes);
	}

    private void handleLeafCollected()
    {
        _leavesLeft--;
        if(_leavesLeft == 0 && !GameManager.instance.IsLevelOver)
        {
            GlobalEvents.WinLevel.Invoke();
        }
    }
}
