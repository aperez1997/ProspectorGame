using System;
using System.Diagnostics;

[Serializable]
public class GameData
{
    public static GameData Instance { get; set; }

    public Player Player { get; private set; }

    public Inventory Inventory { get { return Player.Inventory; } }

    public GameData()
    {       
        // TODO: move this to some sort of "New Game" handler
        Player = new Player(12);
        Player.Inventory.AddItem(new Item { type = Item.ItemType.Ration, amount = 7 });
        Player.ResetActionPoints();
    }
}
