using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera World;
    public Camera UI;

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
    }
}
