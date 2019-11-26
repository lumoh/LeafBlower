using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Leaf : MonoBehaviour
{
    public MeshRenderer Mesh;
    public List<Material> LeafMaterials;

    public const float GUST_INVTERVAL = 1f;
    public const float GUST_FORCE = 2f;


    public Rigidbody rb;
    public BoxCollider col;

    private bool _isGrounded;
    private bool _isCollected;

    public UnityEvent CollectedEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if(Mesh != null && LeafMaterials != null)
        {
            Mesh.material = LeafMaterials[Random.Range(0, LeafMaterials.Count)];
        }

        StartCoroutine(gust());
    }

    public void Blow(Vector3 dir, float force)
    {
        if(rb != null)
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
            int deadzoneMask = 1 << Layers.DEADZONE;
            _isCollected = Physics.Raycast(transform.position, Vector3.down, 0.25f, deadzoneMask);

            if (_isCollected)
            {
                GlobalEvents.LeafCollected.Invoke();
            }
            else
            {
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
}
