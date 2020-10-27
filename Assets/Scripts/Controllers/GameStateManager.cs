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
        player.Inventory.AddItem(new InventoryItem(ItemType.Money, 50));
        player.Inventory.AddItem(new InventoryItem(ItemType.Ration, 7));
        player.Inventory.AddItem(new InventoryItem(ItemType.Pan, 1));

        List<DataTile> DataTileList = WorldMapLoader.CreateRandomWorldMap();

        GameState state = new GameState(player, DataTileList);
        GameState.Instance = state;
    }
}
