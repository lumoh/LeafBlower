using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Leaf : MonoBehaviour
{
    public MeshRenderer Mesh;
    public List<Material> LeafMaterials;

    public const float WIDTH = 0.1f;
    public const float IDLE_TIME = 1f;

    public Rigidbody rb;
    public BoxCollider col;

    [System.NonSerialized] public int ColorInt;

    private bool _isCollected;
    private int _groundMask;
    private float _idleTime;
    private bool _addedAsPointer;
    private bool _blownOnce;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _groundMask = 1 << Layers.GROUND;
        InvokeRepeating("setPlatform", Random.Range(0f, 0.25f), 0.25f);
        rb.drag = 2f;
    }

    public void SetRandomColor()
    {
        if (Mesh != null && LeafMaterials != null)
        {
            ColorInt = Random.Range(0, LeafMaterials.Count);
            Mesh.material = LeafMaterials[ColorInt];
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
            rb.isKinematic = false;
            rb.AddForce(dir * force, ForceMode.Force);
            Vector3 variance = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 5f), Random.Range(-1, 1f));
            rb.AddForce(variance * force / 2f, ForceMode.Force);
            rb.AddTorque(variance);

            _idleTime = 0f;
            if(!_blownOnce)
            {
                _blownOnce = true;
            }
        }
    }

    private void Update()
    {
        if(!_isCollected)
        {
            // much more efficient than raycast
            _isCollected = transform.position.y < -1f;

            if (_isCollected)
            {
                if(GameManager.instance.DestroyWhenCollected)
                {
                    transform.parent = null;
                    Destroy(gameObject);
                }
                else
                {
                    transform.parent = null;
                }

                SoundManager.instance.PlaySFX("bop");
                GlobalEvents.LeafCollected.Invoke();
                GlobalEvents.LeafCollectedInfo.Invoke(this);
            }

            if(rb.isKinematic)
            {
                _idleTime += Time.deltaTime;
                if(_idleTime > IDLE_TIME && !_addedAsPointer && _blownOnce)
                {
                    _addedAsPointer = true;

                    if (GoalPointerManager.instance != null)
                    {
                        GoalPointerManager.instance.AddTarget(this);
                    }
                }
            }
        }
    }

    private void setPlatform()
    {
        if (GameManager.instance.Level != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.15f, _groundMask))
            {
                transform.parent = hit.transform.parent;

                if (rb.velocity.sqrMagnitude < 0.05f && !rb.isKinematic)
                {
                    rb.isKinematic = true;

                    if (_isCollected)
                    {
                        CancelInvoke("setPlatform");
                    }
                }
            }
        }
    }

    public void RemovePointer()
    {
        _addedAsPointer = false;
    }

    public bool IsResting()
    {
        return rb.isKinematic;
    }

    public bool IsCollected()
    {
        return _isCollected;
    }
}
