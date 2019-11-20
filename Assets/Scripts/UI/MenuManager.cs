﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        GlobalEvents.LoseLevel.AddListener(handleLoseGame);
    }

    private void handleLoseGame()
    {
        PushMenu("GameOverMenu");
    }

    public static void PushMenu(string menuId)
    {
        GameObject menuPrefab = Resources.Load("Menus/" + menuId) as GameObject;
        GameObject menu = Instantiate(menuPrefab);
        menu.SetActive(true);
    }
}
