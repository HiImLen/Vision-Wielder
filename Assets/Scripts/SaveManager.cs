using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    public string savePath;
    public string saveName;
    public string saveExtension;
    public string saveFile;

    void Awake()
    {
        savePath = Application.persistentDataPath;
        saveName = "save";
        saveExtension = ".fun";
        saveFile = savePath + "/" + saveName + saveExtension;
    }
    
    public void SaveGameBinary(int level, float timer, int health, float currentExp, int playerLevel, int NALevel, int skillLevel, int burstLevel)
    {
        GameSaveData.SaveData data = new GameSaveData.SaveData();
        data.level = level;
        data.timer = timer;
        data.health = health;
        data.currentExp = currentExp;
        data.playerLevel = playerLevel;
        data.NALevel = NALevel;
        data.skillLevel = skillLevel;
        data.burstLevel = burstLevel;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFile);
        bf.Serialize(file, data);
        file.Close();
    }

    public GameSaveData.SaveData LoadGameBinary()
    {
        if (File.Exists(saveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFile, FileMode.Open);
            GameSaveData.SaveData data = (GameSaveData.SaveData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + saveFile);
            return null;
        }
    }
}
