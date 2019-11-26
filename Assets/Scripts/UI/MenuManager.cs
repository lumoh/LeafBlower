using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public const string GAME_OVER = "GameOverMenu";
    public const string WIN = "WinMenu";
    public const string HOME = "HomeMenu";

    public static void PushMenu(string menuId)
    {
        GameObject menuPrefab = Resources.Load("Menus/" + menuId) as GameObject;
        GameObject menu = Instantiate(menuPrefab);
        menu.SetActive(true);
    }
}
