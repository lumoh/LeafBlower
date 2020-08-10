using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenLeafSpawner : MonoBehaviour
{
    public Leaf LeafPrefab;
    public float Scale;
    public int NumLeaves;

    private List<Leaf> _leaves;

    private void Awake()
    {
        _leaves = new List<Leaf>();
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
    }

    void handleStartLevel()
    {
        InvokeRepeating("spawnLeavesCheck", 0, 1f);
    }

    void spawnLeavesCheck()
    {
        bool allCollected = true;
        foreach(var leaf in _leaves)
        {
            if(!leaf.IsCollected())
            {
                allCollected = false;
                break;
            }
        }

        if(allCollected)
        {
            spawnLeaves();
        }
    }

    void spawnLeaves()
    {
        destroyLeaves();

        if(LeafPrefab != null)
        {
            for(int i = 0; i < NumLeaves; i++)
            {
                var newLeaf = Instantiate(LeafPrefab, transform.parent);
                newLeaf.transform.position = new Vector3(Random.Range(-Scale, Scale), transform.position.y, Random.Range(-Scale, Scale));
                newLeaf.rb.AddTorque(Random.onUnitSphere);
                newLeaf.SetRandomColor();
                _leaves.Add(newLeaf);
            }
        }
    }

    void destroyLeaves()
    {
        if(_leaves != null)
        {
            foreach(var leaf in _leaves)
            {
                if (leaf != null)
                {
                    Destroy(leaf.gameObject);
                }
            }

            _leaves.Clear();
        }
    }

    private void OnDestroy()
    {
        GlobalEvents.StartLevel.RemoveListener(spawnLeaves);
    }
}
