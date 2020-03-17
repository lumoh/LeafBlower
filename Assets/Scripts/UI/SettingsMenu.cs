using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Text version;

    private void Start()
    {
        version.text = "v" + Application.version;
    }

    public void RestorePurchase()
    {
        GameManager.instance.IAP.RestorePurchases();
    }
}
