using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [System.NonSerialized] public int Num;
    public Transform PlayerSpawn;
	public Transform PlatformsParent;
	public Transform LeavesParent;
    public Transform IntroCameraParent;
    [System.NonSerialized] List<Transform> IntroCamPositions;

    public int NumLeaves;
    public int Seconds = 30;
    public bool IsZen;

    private List<Transform> _platforms;
    private List<LeafPile> _leafPiles;
    private int _leavesLeft;

    private FollowCamera _followCamera;

    private void Awake()
    {
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);
        GlobalEvents.LevelShown.AddListener(doLevelPan);

        fetchPlatforms();
        fetchLeaves();
        fetchIntroCamPositions();

        initCamera();

        Seconds = Mathf.Max(60, Seconds);
    }

    private void initCamera()
    {
        Camera worldCam = CameraManager.instance.World;
        _followCamera = worldCam.GetComponent<FollowCamera>();

        if (IntroCamPositions != null && IntroCamPositions.Count > 0)
        {
            _followCamera.FollowTargetOn = false;
            _followCamera.SetTarget(IntroCamPositions[0]);
            _followCamera.MoveToTarget(0);
        }
        else
        {
            _followCamera.SetTarget(GameManager.instance.PlayerObj.transform);
            _followCamera.DistanceDamp = 0;

            if (!GameManager.instance.CheatMenuEnabled)
            {
                MenuManager.PushMenu(MenuManager.HOME);
            }
        }
    }

    private void doLevelPan()
    {
        List<Transform> transforms = new List<Transform>();
        if (IntroCamPositions != null && IntroCamPositions.Count > 1)
        {
            transforms.AddRange(IntroCamPositions.GetRange(1, IntroCamPositions.Count - 1));
        }
        transforms.Add(GameManager.instance.PlayerObj.transform);

        float duration = 1f + transforms.Count;
        _followCamera.PanOverTransforms(duration, transforms, panCallback);
    }

    private void panCallback()
    {
        _followCamera.Target = GameManager.instance.PlayerObj.transform;
        _followCamera.DistanceDamp = 0;
        _followCamera.FollowTargetOn = true;

        if (!GameManager.instance.CheatMenuEnabled)
        {
            MenuManager.PushMenu(MenuManager.HOME);
        }
    }

    private void fetchPlatforms()
	{
        _platforms = new List<Transform>();
        for (int i = 0; i < PlatformsParent.childCount; i++)
        {
            _platforms.Add(PlatformsParent.GetChild(i));
        }
	}

    private void fetchIntroCamPositions()
    {
        if (IntroCameraParent != null && IntroCameraParent.childCount > 0)
        {
            IntroCamPositions = new List<Transform>();
            foreach(Transform t in IntroCameraParent)
            {
                IntroCamPositions.Add(t);
            }
        }
    }

    private void fetchLeaves()
	{
        LeafPile[] piles = LeavesParent.GetComponentsInChildren<LeafPile>();
        _leafPiles = new List<LeafPile>(piles);

        foreach(var leafPile in _leafPiles)
        {
            NumLeaves += leafPile.GetNumLeaves();
        }
        _leavesLeft = NumLeaves;
    }

    private void handleLeafCollected()
    {
        _leavesLeft--;
        if(_leavesLeft == 0 && !GameManager.instance.IsLevelOver)
        {
            if(!GameManager.IsZenLevel())
            {
                GlobalEvents.WinLevel.Invoke();
            }            
        }
    }
}
