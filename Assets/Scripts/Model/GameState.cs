using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as the source of truth for game data while running and as the serialized data struct.
/// </summary>
[Serializable]
public class GameState
{
    /// <summary>
    /// Meta data about the game
    /// </summary>
    public GameStateMeta GameStateMeta;
    public DateTime GameDate { get { return GameStateMeta.GameDate; } }

    /// <summary>
    /// The player's data, including their inventory
    /// </summary>
    public Player Player;
    public Inventory Inventory { get { return Player.Inventory; } }

    /// <summary>
    /// The world map.
    /// </summary>
    public WorldMap WorldMap;

    public GameState(GameStateMeta meta, Player player, WorldMap worldMap)
    {
        this.GameStateMeta = meta;
        this.Player = player;
        this.WorldMap = worldMap;
    }

    public WorldTile GetTileForPlayerLocation()
    {
        return WorldMap.GetTileForPlayer(Player);
    }

    public override string ToString()
    {
        return "GameState[P:" + Player.ToString() +"]";
    }
}
