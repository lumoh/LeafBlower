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

    private Vector3 _originalPos;
    private bool _moveFlip;

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;

        StartCoroutine(initialDelay());
    }

    IEnumerator initialDelay()
    {
        yield return new WaitForSeconds(InitialDelay);
        startMovement();
    }

    IEnumerator waitToMove()
    {
        yield return new WaitForSeconds(PauseDuration);
        startMovement();
    }

    void startMovement()
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
}
