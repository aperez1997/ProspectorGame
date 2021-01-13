using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains singleton for current GameLogic, which is how the other classes accesss all game data
/// It does this by acting as a wrapper around SaveSystem.
/// </summary>
public static class GameStateManager
{
    // Logic singleton
    public static GameLogic LogicInstance { get; set; }

    public static void CreateNewGame()
    {
        Debug.Log("Creating gameLogic for new game...");
        var logic = new GameLogic(ScriptedObjectBinder.Instance);
        Debug.Log("GameLogic created successfully. Calling init...");
        logic.InitNewGame();
        LogicInstance = logic;
    }

    public static void SaveGame()
    {
        SaveSystem.SaveData(LogicInstance.GameState);
    }

    public static bool LoadGame()
    {
        GameState state = SaveSystem.LoadData();
        if (state == null) {
            Debug.LogError("Failed to load game");
            return false;
        } else {
            var logic = new GameLogic(state, ScriptedObjectBinder.Instance);
            LogicInstance = logic;
            return true;
        }
    }

    // Hack to auto-load game if we get here and there isn't something loaded yet
    // this should only be for the editor
    public static GameLogic DebugLoadState()
    {
        if (LogicInstance == null)
        {
            Debug.Log("No GameLogic, loading game as an editor hack");
            if (!LoadGame())
            {
                Debug.Log("No saved game. Creating new game as an editor hack");
                CreateNewGame();
            }
        }
        return LogicInstance;
    }
}
