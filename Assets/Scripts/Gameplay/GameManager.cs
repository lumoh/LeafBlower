using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TargetFrameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
