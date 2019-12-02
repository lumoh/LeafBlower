using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FakeAd : MonoBehaviour
{
    public float Delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(close());
    }

    IEnumerator close()
    {
        yield return new WaitForSeconds(Delay);
        Destroy(gameObject);
        GameManager.instance.LoadLevelAndPlayer();
    }
}
