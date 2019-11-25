using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public const string GAME_OVER = "GameOverMenu";
    public const string WIN = "WinMenu";

    private void Start()
    {
        GlobalEvents.LoseLevel.AddListener(handleLoseGame);
    }

    private void handleLoseGame()
    {
        PushMenu(GAME_OVER);
    }

    public static void PushMenu(string menuId)
    {
        GameObject menuPrefab = Resources.Load("Menus/" + menuId) as GameObject;
        GameObject menu = Instantiate(menuPrefab);
        menu.SetActive(true);
    }
}
