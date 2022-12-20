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

    public void SaveGameJson(int level, float timer, int health)
    {
        GameSaveData.SaveData data = new GameSaveData.SaveData();
        data.level = level;
        data.timer = timer;
        data.health = health;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFile, json);
    }

    public GameSaveData.SaveData LoadGameJson()
    {
        string json = File.ReadAllText(saveFile);
        GameSaveData.SaveData data = JsonUtility.FromJson<GameSaveData.SaveData>(json);
        return data;
    }
    
    public void SaveGameBinary(int level, float timer, int health)
    {
        GameSaveData.SaveData data = new GameSaveData.SaveData();
        data.level = level;
        data.timer = timer;
        data.health = health;
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
