using UnityEngine;

// Combines GameState and SaveSystem
public static class GameStateManager
{
    public static bool LoadGame()
    {
        GameState state = SaveSystem.LoadData();
        if (state == null)
        {
            Debug.LogError("Failed to load game");
            return false;
        }
        else
        {
            GameState.Instance = state;
            return true;
        }
    }

    public static void CreateNewGame()
    {
        GameState state = new GameState();
        GameState.Instance = state;
    }
}
