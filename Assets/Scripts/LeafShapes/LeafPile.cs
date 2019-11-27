using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for a cluster of leaves.  Can be shapes or just a pile.
/// </summary>
public class LeafPile : MonoBehaviour
{
    public Leaf LeafPrefab;
    public int NumLeaves;

    protected List<Leaf> _leaves;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _leaves = new List<Leaf>();

        for (int i = 0; i < NumLeaves; i++)
        {
            Leaf newLeaf = spawnLeaf();
            if(newLeaf != null)
            {
                _leaves.Add(newLeaf);
            }
        }
    }

    public virtual int GetNumLeaves()
    {
        return NumLeaves;
    }

    public int NumLeavesLeft()
    {
        return _leaves.Count;
    }

    protected Leaf spawnLeaf()
    {
        Leaf leaf = null;
        if(LeafPrefab != null)
        {
            leaf = Instantiate(LeafPrefab, transform);
            leaf.transform.localPosition = Vector3.zero;
            leaf.transform.localRotation = Quaternion.identity;
        }

        return leaf;
    }
}
