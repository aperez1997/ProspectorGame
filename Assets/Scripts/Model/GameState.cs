using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public static GameState Instance { get; set; }

    public Player Player;

    public Inventory Inventory { get { return Player.Inventory; } }

    public List<DataTile> DataTileList;

    public GameState()
    {       
        // TODO: move this to some sort of "New Game" handler
        Player = new Player(12);
        Player.Inventory.AddItem(new Item { type = Item.ItemType.Ration, amount = 7 });
        Player.ResetActionPoints();

        DataTileList = WorldMapLoader.CreateRandomWorldMap();
    }

    public override string ToString()
    {
        return "GameState[P:" + Player.ToString() +"]";
    }
}
