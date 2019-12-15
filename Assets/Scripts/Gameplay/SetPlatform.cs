using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will attach the gameobject to the ground platform it rests on.
/// This is useful for moving platforms.
/// </summary>
public class SetPlatform : MonoBehaviour
{
    private int _groundMask;

    // Start is called before the first frame update
    void Start()
    {
        _groundMask = 1 << Layers.GROUND;
    }

    // Update is called once per frame
    void Update()
    {
        setPlatform();
    }

    private void setPlatform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 1.2f, _groundMask))
        {
            transform.parent = hit.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
}
