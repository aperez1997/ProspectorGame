using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for getting gameData instance into correct state at the beginning
public class GameDataController : MonoBehaviour
{
    private void Awake()
    {
        GameData data = SaveSystem.LoadData();
        if (data == null)
        {
            Debug.Log("Creating blank gamedata");
            data = new GameData();
        } else
        {
            Debug.Log("Loaded gamedata");
        }
        GameData.Instance = data;
    }
}
