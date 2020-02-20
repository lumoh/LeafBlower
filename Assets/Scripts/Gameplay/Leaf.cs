﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Leaf : MonoBehaviour
{
    public MeshRenderer Mesh;
    public List<Material> LeafMaterials;

    public const float GUST_INVTERVAL = 1f;
    public const float GUST_FORCE = 2f;
    public const float WIDTH = 0.1f;
    public const float IDLE_TIME = 5f;

    public Rigidbody rb;
    public BoxCollider col;

    private bool _isGrounded;
    private bool _isCollected;
    private int _groundMask;

    // Start is called before the first frame update
    void Start()
    {
        _groundMask = 1 << Layers.GROUND;

        //StartCoroutine(gust());

        if (!GameManager.instance.ParticlesEnabled)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetRandomColor()
    {
        if (Mesh != null && LeafMaterials != null)
        {
            Mesh.material = LeafMaterials[Random.Range(0, LeafMaterials.Count)];
        }
    }

    public void SetColor(Color c)
    {
        if (Mesh != null)
        {
            Mesh.material.SetColor("_Color", c);
        }
    }

    public void Blow(Vector3 dir, float force)
    {
        if(rb != null && !_isCollected)
        {
            rb.AddForce(dir * force, ForceMode.Force);
            Vector3 variance = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 5f), Random.Range(-1, 1f));
            rb.AddForce(variance * force / 2f, ForceMode.Force);
            rb.AddTorque(variance);
        }
    }

    private void Update()
    {
        if(!_isCollected)
        {
            setPlatform();

            int deadzoneMask = 1 << Layers.DEADZONE;
            _isCollected = Physics.Raycast(transform.position, Vector3.down, 1.5f, deadzoneMask);

            if (_isCollected)
            {
                if(GameManager.instance.DestroyWhenCollected)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.parent = GameManager.instance.Level.LeavesParent;
                }
                GlobalEvents.LeafCollected.Invoke();
            }
            else
            {
                /*
                int groundMask = 1 << Layers.GROUND;
                int leafMask = 1 << Layers.LEAF;
                int layerMask = groundMask | leafMask;

                _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.25f, layerMask);

                if (_isGrounded)
                {
                    rb.drag = 0.1f;
                }
                else
                {
                    rb.drag = 3.5f;
                }
                */
            }
        }
    }

    private IEnumerator gust()
    {
        yield return new WaitForSeconds(GUST_INVTERVAL);

        if(rb != null && !_isGrounded && !_isCollected)
        {
            Vector3 variance = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0f, GUST_FORCE), Random.Range(-0.1f, 0.1f));
            rb.AddForce(variance * GUST_FORCE / 2f, ForceMode.Impulse);
            rb.AddTorque(variance);
        }

        if (_isCollected)
        {
            StartCoroutine(gust());
        }
    }

    private void setPlatform()
    {
        if (GameManager.instance.Level != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, _groundMask))
            {
                transform.parent = hit.transform.parent;
            }
        }
    }
}
