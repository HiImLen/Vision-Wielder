using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSaveData
{
    [Serializable]
    public class SaveData
    {
        public int level;
        public float timer;
        public int health;
    }

    public static void SaveGame(int level, float timer, int health)
    {
        SaveData data = new SaveData();
        data.level = level;
        data.timer = timer;
        data.health = health;
    }
}
