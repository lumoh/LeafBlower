using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public Transform SpawnPoint;
    public float MaxForce = 5f;
    public float MaxDistance = 1f;
    public Animator Anim;
    public ParticleSystem BlowParticles;

    private const string BLOW_ANIM = "Blow";
    private const string IDLE_ANIM = "Idle";

    private void Awake()
    {
        GlobalEvents.StartLevel.AddListener(startBlower);
        GlobalEvents.LoseLevel.AddListener(stopBlower);
        GlobalEvents.WinLevel.AddListener(stopBlower);
    }

    private void startBlower()
    {
        if(Anim != null)
        {
            Anim.Play(BLOW_ANIM);
        }

        if(BlowParticles != null)
        {
            BlowParticles.Play();
        }
    }

    private void stopBlower()
    {
        if (Anim != null)
        {
            Anim.Play(IDLE_ANIM);
        }

        if (BlowParticles != null)
        {
            BlowParticles.Stop();
        }
    }


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
