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

    [SerializeField] protected List<Leaf> _leaves;

    private Leaf _pointer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        SetColors();
    }

    public List<Leaf> GetLeaves()
    {
        return _leaves;
    }

    /// <summary>
    /// Set random colors on the blocks
    /// </summary>
    public virtual void SetColors()
    {
        foreach(var leaf in _leaves)
        {
            leaf.SetRandomColor();
        }
    }

    public virtual int GetNumLeaves()
    {
        return NumLeaves;
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

    private void OnDestroy()
    {
        foreach(var leaf in _leaves)
        {
            if (leaf != null)
            {
                Destroy(leaf.gameObject);
            }
        }
    }
}
