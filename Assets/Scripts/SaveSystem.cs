
using System.IO;
// for later binary serialization
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = GetSaveFilePath();
        File.WriteAllText(path, json);
    }

    public static GameData LoadData()
    {
        string path = GetSaveFilePath();
        if (!File.Exists(path))
        {
            Debug.LogError("Save file " + path + " not found");
            return null;
        }

        string rawData = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(rawData);
        return data;
    }

    // TODO: what about multiple saves?
    private static string GetSaveFilePath()
    {
        return Application.persistentDataPath + "/miner.save";
    }
}
