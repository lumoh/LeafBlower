using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float LeadOffset;
    public float DistanceDamp;

    [HideInInspector]
    public Vector3 Velocity;

    private void Awake()
    {

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

    void LateUpdate()
    {
        followSmoothDamp();
    }

    void followSmoothDamp()
    {
        if (Target != null)
        {
            Vector3 toPos = (Target.position + (Target.forward * LeadOffset)) + Offset;
            Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref Velocity, DistanceDamp);
            transform.position = curPos;
        }
    }

    private void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.DrawLine(Target.position, Target.position + Target.forward * 2);
        }
    }
}
