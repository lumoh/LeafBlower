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
        GlobalEvents.StartLevel.AddListener(handleStartLevel);
        GlobalEvents.RetryLevel.AddListener(handleStartLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.WinLevel.AddListener(handleLoseLevel);
    }

    private void handleStartLevel()
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

    private void handleLoseLevel()
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
