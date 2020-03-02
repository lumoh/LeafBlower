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
    private const float HAPTICS_INTERVAL_LOW = 0.15f;
    private const float HAPTICS_INTERVAL_HIGH = 0.075f;
    private const int HAPTICS_THRESHOLD_COUNT = 50;

    private bool _isBlowing;
    private int _leafMask;
    //private int _groundMask;
    private bool _hittingLeaves;
    private float _hapticsTime;
    private bool _lowHaptics;
    

    private void Awake()
    {
        GlobalEvents.StartLevel.AddListener(startBlower);
        GlobalEvents.LoseLevel.AddListener(stopBlower);
        GlobalEvents.WinLevel.AddListener(stopBlower);

        _leafMask = 1 << Layers.LEAF;
        //_groundMask = 1 << Layers.GROUND;
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

        _isBlowing = true;
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

        _isBlowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBlowing)
        {
            RaycastHit[] hits = Physics.SphereCastAll(SpawnPoint.position, 1f, SpawnPoint.forward, MaxDistance, _leafMask);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    Leaf leaf = hit.collider.GetComponent<Leaf>();
                    if (leaf != null)
                    {
                        bool blocked = false;

                        /*
                        RaycastHit hitInfo;
                        if (Physics.Raycast(leaf.transform.position, SpawnPoint.position - leaf.transform.position, out hitInfo, MaxDistance, _groundMask))
                        {
                            blocked = true;
                        }

                        if(hit.distance < 0.05f)
                        {
                            blocked = false;
                        }
                        */

                        if(!blocked)
                        {
                            leaf.Blow(SpawnPoint.forward, MaxForce);
                            _hittingLeaves = true;
                        }
                    }
                }
            }
            else
            {
                _hittingLeaves = false;
            }

            if (Taptic.tapticOn)
            {
                _lowHaptics = hits.Length < HAPTICS_THRESHOLD_COUNT;
                if (_hittingLeaves)
                {
                    _hapticsTime += Time.deltaTime;
                    if (_lowHaptics && _hapticsTime > HAPTICS_INTERVAL_LOW)
                    {
                        _hapticsTime = 0f;
                        Taptic.Medium();
                    }
                    else if (!_lowHaptics && _hapticsTime > HAPTICS_INTERVAL_HIGH)
                    {
                        _hapticsTime = 0f;
                        Taptic.Medium();
                    }
                }
            }
        }
    }
}
