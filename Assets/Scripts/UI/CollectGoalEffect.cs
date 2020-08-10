using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoalEffect : MonoBehaviour
{
    public SpriteRenderer sr;
    public ParticleSystem ps;

    public void Init(Color c)
    {
        sr.color = c;
        var main = ps.main;
        main.startColor = c;
    }
}
