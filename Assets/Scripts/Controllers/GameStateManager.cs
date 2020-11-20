using System.Collections.Generic;
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
        // TODO: move this to another class? 
        // GameLogic would be nice, but currently gameLogic requires gameState, which creates 
        // chicken/egg problem.
        Player player = new Player(12);
        player.Inventory.AddItem(ItemId.Money, 50);
        player.Inventory.AddItem(ItemId.Ration, 7);
        player.Inventory.AddItem(ItemId.Pan, 1);

        List<WorldTile> TileList = WorldMapLoader.CreateRandomWorldMap();

        GameState state = new GameState(player, TileList);
        GameState.Instance = state;

        // reveal players location
        var tileAt = state.GetTileForPlayerLocation(player);
        tileAt.Reveal(1);
    }

    // Hack to auto-load game if we get here and there isn't something loaded yet
    // this should only be for the editor
    public static GameState DebugLoadState()
    {
        if (GameState.Instance == null)
        {
            Debug.Log("No Gamestate, loading game as an editor hack");
            if (!LoadGame())
            {
                Debug.Log("No saved game. Creating new game as an editor hack");
                CreateNewGame();
            }
        }
        return GameState.Instance;
    }
}
