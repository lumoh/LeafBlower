using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float LeadOffset;
    public float DistanceDamp;

    [System.NonSerialized] public Vector3 Velocity;
    [System.NonSerialized] public bool FollowTargetOn;

    private void Awake()
    {
        FollowTargetOn = true;
    }

    public void SetTarget(Transform t)
    {
        Target = t;
        if (Target != null)
        {
            Vector3 toPos = Target.position + Offset;
            transform.position = toPos;
            transform.LookAt(Target.position, Target.up);
        }
    }

    public void MoveToTarget(float duration)
    {
        if(Target != null)
        {
            transform.DOMove(GetToPos(), duration);
        }
    }

    public void PanOverTransforms(float duration, List<Transform> transforms, TweenCallback cb = null)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach(var t in transforms)
        {
            Vector3 toPos = (t.position + (t.forward * LeadOffset)) + Offset;
            positions.Add(toPos);
        }

        transform.DOPath(positions.ToArray(), duration).SetEase(Ease.InOutSine).OnComplete(cb);
    }

    void LateUpdate()
    {
        if (FollowTargetOn)
        {
            followSmoothDamp();
        }
    }

    void followSmoothDamp()
    {
        if (Target != null)
        {
            Vector3 toPos = GetToPos();
            Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref Velocity, DistanceDamp);
            transform.position = curPos;
        }
    }

    public Vector3 GetToPos()
    {
        Vector3 toPos = (Target.position + (Target.forward * LeadOffset)) + Offset;
        return toPos;
    }

    private void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.DrawLine(Target.position, Target.position + Target.forward * 2);
        }
    }
}
