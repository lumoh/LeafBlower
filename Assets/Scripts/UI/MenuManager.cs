using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public const string GAME_OVER = "GameOverMenu";
    public const string WIN = "WinMenu";
    public const string HOME = "HomeMenu";
    public const string LOADING_MENU = "LoadingMenu";
    public const string FAKE_AD = "FakeAd";
    public const string SETTINGS = "SettingsMenu";
    public const string SUCCESS = "SuccessMenu";
    public const string FAILED = "FailedMenu";

    private static LoadingMenu _loadingMenu;

    public static GameObject PushMenu(string menuId)
    {
        GameObject menuPrefab = Resources.Load("Menus/" + menuId) as GameObject;
        GameObject menu = Instantiate(menuPrefab);
        menu.name = menuId;
        menu.SetActive(true);
        return menu;
    }

    public static void PopMenu(GameObject gameObj)
    {
        string name = gameObj.name;
        Destroy(gameObj);
        GlobalEvents.MenuPopped.Invoke(name);
    }

    public static void ShowLoadingScreen(TweenCallback callback = null)
    {
        if(_loadingMenu == null)
        {
            _loadingMenu = MenuManager.PushMenu(LOADING_MENU).GetComponent<LoadingMenu>();
            _loadingMenu.Init(callback);
        }
        else if(callback != null)
        {
            callback.Invoke();
        }
    }

    public static void RemoveLoadingScreen()
    {
        if(_loadingMenu != null)
        {
            _loadingMenu.FadeOut();
        }
    }
}
