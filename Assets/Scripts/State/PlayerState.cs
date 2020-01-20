using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// PlayerState save to player prefs
/// </summary>
public class PlayerState
{
    public const string LEVEL = "lvl";
    public const string HAPTICS = "haptics";

    public static int GetLevel()
    {
        int level = Mathf.Max(PlayerPrefs.GetInt(LEVEL), 1);
        return level;
    }

    public static void SetLevel(int levelNum)
    {
        PlayerPrefs.SetInt(LEVEL, levelNum);
        PlayerPrefs.Save();
    }

    public static void IncrementLevel()
    {
        int newLevelNum = GetLevel() + 1;
        SetLevel(newLevelNum);
    }

    public static void ClearData()
    {
        PlayerPrefs.DeleteKey(LEVEL);
    }

    public static bool GetBool(string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }

    public static void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }


#if UNITY_EDITOR
    [MenuItem("Blower/Clear Player State")]
    static void ClearPlayerState()
    {
        ClearData();
    }
#endif
}
