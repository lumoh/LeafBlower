using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRecord
{
    public int LevelNum;
    public int Attempts;
    public float SecondsLeft;

    public LevelRecord(int levelNum)
    {
        LevelNum = levelNum;
        Attempts = 1;
        SecondsLeft = 0;
    }
}
