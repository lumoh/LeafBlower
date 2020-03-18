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
        GlobalEvents.PurchaseComplete.AddListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.AddListener(handlePurchaseFailed);

        MenuManager.ShowLoadingScreen();

        GameManager.instance.IAP.RestorePurchases();
    }

    private void handlePurchaseSuccess()
    {
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);

        MenuManager.RemoveLoadingScreen();
    }

    private void handlePurchaseFailed()
    {
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);
        
        MenuManager.RemoveLoadingScreen();
    }

    private void OnDestroy()
    {
        GlobalEvents.PurchaseComplete.RemoveListener(handlePurchaseSuccess);
        GlobalEvents.PurchaseFailed.RemoveListener(handlePurchaseFailed);
    }
}
