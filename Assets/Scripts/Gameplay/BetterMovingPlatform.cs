using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BetterMovingPlatform : MovingPlatform
{
    public List<PlatformMovement> Movements;
    public bool Loop;

    private int _moveIndex;
    private PlatformMovement _currentMovement;

    protected override IEnumerator waitToMove()
    {
        _currentMovement = Movements[_moveIndex];
        yield return new WaitForSeconds(_currentMovement.Delay);
        startMovement();
    }

    protected override void startMovement()
    {
        _currentMovement = Movements[_moveIndex];
        Vector3 pos = _originalPos + _currentMovement.Offset;
        _moveIndex = (_moveIndex + 1) % Movements.Count;

        transform.DOMove(pos, _currentMovement.Duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            StartCoroutine(waitToMove());
        });
    }
}
