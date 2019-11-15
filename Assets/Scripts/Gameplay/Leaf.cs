using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public const float GUST_INVTERVAL = 1f;
    public const float GUST_FORCE = 2f;

    private Rigidbody rb;
    private bool _isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
        int layerMask = 1 << 8;
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

    private IEnumerator gust()
    {
        yield return new WaitForSeconds(GUST_INVTERVAL);

        if(rb != null && !_isGrounded)
        {
            Vector3 variance = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0f, GUST_FORCE), Random.Range(-0.1f, 0.1f));
            rb.AddForce(variance * GUST_FORCE / 2f, ForceMode.Impulse);
            rb.AddTorque(variance);
        }

        StartCoroutine(gust());
    }
}
