using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to empty gameobjet to create a cube of leaves
/// </summary>
public class LeafCube : LeafPile
{
    public Vector3Int Dimensions;

    // Start is called before the first frame update
    public override void Start()
    {
        _leaves = new List<Leaf>();

        float startX = (Dimensions.x * Leaf.WIDTH) / -2f;
        float startZ = (Dimensions.z * Leaf.WIDTH) / -2f;
        float startY = Leaf.WIDTH / 2f;

        if (LeafPrefab != null)
        {
            for (int y = 0; y < Dimensions.y; y++)
            { 
                for (int x = 0; x < Dimensions.x; x++)
                {
                    for (int z = 0; z < Dimensions.z; z++)
                    {
                        Leaf newLeaf = spawnLeaf();
                        float xPos = startX + (x * Leaf.WIDTH);
                        float zPos = startZ + (z * Leaf.WIDTH);
                        float yPos = startY + (y * Leaf.WIDTH);
                        newLeaf.transform.localPosition = new Vector3(xPos, yPos, zPos);
                        newLeaf.transform.localRotation = Quaternion.identity;
                        _leaves.Add(newLeaf);
                    }
                }
            }
        }
    }

    public override int GetNumLeaves()
    {
        return Dimensions.x * Dimensions.y * Dimensions.z;
    }
}
