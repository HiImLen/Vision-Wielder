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
        public float currentExp;
        public int playerLevel;
        public int NALevel;
        public int skillLevel;
        public int burstLevel;
    }

    public static void SaveGame(int level, float timer, int health, float currentExp, int playerLevel, int NALevel, int skillLevel, int burstLevel)
    {
        SaveData data = new SaveData();
        data.level = level;
        data.timer = timer;
        data.health = health;
        data.currentExp = currentExp;
        data.playerLevel = playerLevel;
        data.NALevel = NALevel;
        data.skillLevel = skillLevel;
        data.burstLevel = burstLevel;
    }
}
