using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Text version;
    public SwitchManager MusicSwitch;
    public SwitchManager SFXSwitch;

    private void Start()
    {
        version.text = "v" + Application.version;

        setMusic();
        setSFX();
    }

    void setMusic()
    {
        MusicSwitch.isOn = !PlayerState.GetBool(PlayerState.MUSIC_MUTED);
    }

    void setSFX()
    {
        SFXSwitch.isOn = !PlayerState.GetBool(PlayerState.SFX_MUTED);
    }

    public void MusicOff()
    {
        PlayerState.SetBool(PlayerState.MUSIC_MUTED, true);
        SoundManager.instance.MuteMusic(true);
        SoundManager.Button();
    }

    public void MusicOn()
    {
        PlayerState.SetBool(PlayerState.MUSIC_MUTED, false);
        SoundManager.instance.MuteMusic(false);
        SoundManager.Button();
    }

    public void SFXOn()
    {
        PlayerState.SetBool(PlayerState.SFX_MUTED, false);
        SoundManager.instance.MuteSFX(false);
        SoundManager.Button();
    }

    public void SFXOff()
    {
        PlayerState.SetBool(PlayerState.SFX_MUTED, true);
        SoundManager.instance.MuteSFX(true);
        SoundManager.Button();
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
