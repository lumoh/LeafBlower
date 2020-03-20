﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [System.NonSerialized] public int Num;
    public Transform PlayerSpawn;
	public Transform PlatformsParent;
	public Transform LeavesParent;

    public int NumLeaves;
    public int Seconds = 30;
    public bool IsZen;

    private List<Transform> _platforms;
    private List<LeafPile> _leafPiles;
    private int _leavesLeft;

    private void Awake()
    {
        GlobalEvents.LeafCollected.AddListener(handleLeafCollected);

        fetchPlatforms();
        fetchLeaves();
    }

    private void fetchPlatforms()
	{
        _platforms = new List<Transform>();
        for (int i = 0; i < PlatformsParent.childCount; i++)
        {
            _platforms.Add(PlatformsParent.GetChild(i));
        }
	}

    public List<Leaf> GetPointers()
    {
        List<Leaf> pointers = new List<Leaf>();
        if (_leafPiles != null)
        {
            foreach (var pile in _leafPiles)
            {
                Leaf pointer = pile.GetPointer();
                pointers.Add(pointer);
            }
        }

        return pointers;
    }


    public Leaf GetRandomLeaf()
    {
        Leaf leaf = _leafPiles[0].GetLeaves()[0];
        return leaf;
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
