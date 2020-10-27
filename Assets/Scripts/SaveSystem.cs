
using System.IO;
using UnityEngine;

public static class SaveSystem 
{
    enum SaveMode { JSON, Binary };
    const SaveMode CurrentMode = SaveMode.JSON;

    public static void SaveData(GameState state)
    {
        Debug.Log("State is " + state.ToString());

        string json = JsonUtility.ToJson(state);
        string path = GetSaveFilePath();
        Debug.Log("serialized data is " + json);
        File.WriteAllText(path, json);
    }

    public static bool HasGame()
    {
        string path = GetSaveFilePath();
        return File.Exists(path);
    }

    public static GameState LoadData()
    {
        string path = GetSaveFilePath();
        if (!File.Exists(path))
        {
            Debug.LogError("Save file " + path + " not found");
            return null;
        }

        string rawData = File.ReadAllText(path);
        GameState state = JsonUtility.FromJson<GameState>(rawData);
        return state;
    }

    // TODO: what about multiple saves?
    private static string GetSaveFilePath()
    {
        string ext = GetExtension();
        string path = Application.persistentDataPath + "/miner." + ext;
        return path;
    }
    private static string GetExtension()
    {
        switch (CurrentMode)
        {
            case SaveMode.JSON:
                return "json";
            case SaveMode.Binary:
                //return "bin";
        }
    }
}
