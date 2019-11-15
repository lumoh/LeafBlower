using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public Transform SpawnPoint;
    public float MaxForce = 5f;
    public float MaxDistance = 1f;

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 9;
        
        RaycastHit[] hits = Physics.SphereCastAll(SpawnPoint.position, 1f, SpawnPoint.forward, 2f, layerMask);
        foreach(RaycastHit hit in hits)
        {
            Leaf leaf = hit.collider.GetComponent<Leaf>();
            if(leaf != null)
            {
                leaf.Blow(SpawnPoint.forward, MaxForce);
            }
        }
    }
}
