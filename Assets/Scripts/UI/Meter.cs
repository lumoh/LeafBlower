using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    public Image MeterFill;

    private float _fillAmount;

    public void SetFill(float fillAmount)
    {
        _fillAmount = fillAmount;
        MeterFill.fillAmount = fillAmount;
    }
}
