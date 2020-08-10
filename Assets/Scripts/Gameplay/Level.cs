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
    private int _introIndex;

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

        fetchPlatforms();
        fetchLeaves();
        fetchIntroCamPositions();

        Seconds = Mathf.Max(60, Seconds);
    }

    public void DoLevelPan()
    {
        if(IntroCamPositions != null && IntroCamPositions.Count > 0)
        {
            _introIndex = 0;
            Camera worldCam = CameraManager.instance.World;
            _followCamera = worldCam.GetComponent<FollowCamera>();
            _followCamera.SetTarget(IntroCamPositions[0]);
            _followCamera.DistanceDamp = 0.75f;
            StartCoroutine(panCoroutine());
        }
        else
        {
            Camera worldCam = CameraManager.instance.World;
            _followCamera = worldCam.GetComponent<FollowCamera>();
            _followCamera.SetTarget(GameManager.instance.PlayerObj.transform);
            _followCamera.DistanceDamp = 0;
            MenuManager.PushMenu(MenuManager.HOME);
        }
    }

    IEnumerator panCoroutine()
    {
        if(_introIndex == 0)
        {
            yield return new WaitForSeconds(2.85f);
        }

        _introIndex++;
        if(_introIndex < IntroCamPositions.Count)
        {
            _followCamera.Target = IntroCamPositions[_introIndex];
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(panCoroutine());
        }
        else
        {
            _followCamera.Target = GameManager.instance.PlayerObj.transform;
            yield return new WaitForSeconds(3f);
            _followCamera.DistanceDamp = 0;
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
