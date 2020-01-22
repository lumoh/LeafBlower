using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 Offset;
    public float MoveDuration = 5;
    public float PauseDuration = 0;
    public float InitialDelay = 0f;
    public bool WaitForStart;

    protected Vector3 _originalPos;
    private bool _moveFlip;

    protected Vector3 _lastPos;
    protected Vector3 _velocity;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _originalPos = transform.position;

        if (!WaitForStart)
        {
            StartCoroutine(initialDelay());
        }
        else
        {
            GlobalEvents.StartLevel.AddListener(handleLevelStart);
        }
    }

    protected virtual void handleLevelStart()
    {
        GlobalEvents.StartLevel.RemoveListener(handleLevelStart);
        StartCoroutine(initialDelay());
    }

    protected virtual IEnumerator initialDelay()
    {
        yield return new WaitForSeconds(InitialDelay);
        startMovement();
    }

    protected virtual IEnumerator waitToMove()
    {
        yield return new WaitForSeconds(PauseDuration);
        startMovement();
    }

    protected virtual void startMovement()
    {
        Vector3 pos = _originalPos + Offset;
        if (_moveFlip)
        {
            pos = _originalPos;
        }

        transform.DOMove(pos, MoveDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            StartCoroutine(waitToMove());
        });

        _moveFlip = !_moveFlip;
    }

    protected void Update()
    {
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}
