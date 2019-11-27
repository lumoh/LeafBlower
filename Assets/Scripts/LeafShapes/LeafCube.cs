using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to empty gameobjet to create a cube of leaves
/// </summary>
public class LeafCube : MonoBehaviour
{
    public Leaf LeafPrefab;
    public Vector3Int Dimensions;

    private const float WIDTH = 0.1f;
    private List<Leaf> leaves;

    // Start is called before the first frame update
    void Start()
    {
        leaves = new List<Leaf>();
        if (LeafPrefab != null)
        {
            for (int y = 0; y < Dimensions.y; y++)
            { 
                for (int x = 0; x < Dimensions.x; x++)
                {
                    for (int z = 0; z < Dimensions.z; z++)
                    {
                        Leaf leafObj = Instantiate(LeafPrefab, transform);
                        leafObj.transform.localPosition = new Vector3(x * WIDTH, (y * WIDTH) + (WIDTH/2f), z * WIDTH);
                        leafObj.transform.localRotation = Quaternion.identity;
                        leaves.Add(leafObj);
                    }
                }
            }
        }
    }

    public List<Leaf> GetLeaves()
	{
		return leaves;
	}

    public int GetNumLeaves()
    {
        return Dimensions.x * Dimensions.y * Dimensions.z;
    }
}
