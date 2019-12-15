using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 Offset;
    public float MoveDuration = 5;
    public float PauseDuration = 0;

    private Vector3 _originalPos;
    private bool _moveFlip;

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;

        StartCoroutine(waitToMove());
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

    IEnumerator waitToMove()
    {
        yield return new WaitForSeconds(PauseDuration);
        startMovement();
    }
}
