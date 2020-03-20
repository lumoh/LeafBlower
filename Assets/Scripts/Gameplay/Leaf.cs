using System.Collections;
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

    public UnityEvent DestroyedEvent = new UnityEvent();

    private bool _isCollected;
    private int _groundMask;
    private GameObject _covidObj;

    void Awake()
    {
        if(GameManager.instance.CovidPrefab != null)
        {
            _covidObj = Instantiate(GameManager.instance.CovidPrefab, transform);
            _covidObj.transform.localRotation = Quaternion.Euler(Random.onUnitSphere * 360f);
            Mesh.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _groundMask = 1 << Layers.GROUND;

        if (GameManager.instance.ParticlesEnabled)
        {            
            var ps = transform.GetChild(0).GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = Color.green;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        InvokeRepeating("setPlatform", Random.Range(0f, 0.25f), 0.25f);
        rb.drag = 2f;
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
            rb.isKinematic = false;
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
            // much more efficient than raycast
            _isCollected = transform.position.y < -1f;

            if (_isCollected)
            {
                if(GameManager.instance.DestroyWhenCollected)
                {                    
                    Destroy(gameObject, 2f);
                }
                else
                {
                    transform.parent = GameManager.instance.Level.LeavesParent;
                }

                SoundManager.instance.PlaySFX("tick2");
                GlobalEvents.LeafCollected.Invoke();
                DestroyedEvent.Invoke();
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

    public bool IsCollected()
    {
        return _isCollected;
    }
}
