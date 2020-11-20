using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic
{
    private Player Player { get { return gameState.Player; } }

    private Inventory Inventory { get { return Player.Inventory; } }

    private GameStateMeta GameStateMeta { get { return gameState.GameStateMeta; } }

    private readonly GameState gameState;

    public GameLogic(GameState gameState)
    {
        this.gameState = gameState;
    }

    // Movement

    /// <summary>
    /// Gets AP cost to move from tileAt to tileNeighbor
    /// </summary>
    public int GetMovementCost(WorldTile tileAt, WorldTile tileNeighbor)
    {
        bool hasRoad = tileAt.HasRoad();
        int costNeighbor = tileNeighbor.GetMoveCost(hasRoad);
        costNeighbor = Math.Max(1, costNeighbor);
        return costNeighbor;
    }

    /// <summary>
    /// Move player from tileFrom to tileTo
    /// </summary>
    public bool MovePlayer(WorldTile tileFrom, HexDirection direction)
    {
        var tileTo = tileFrom.GetNeighborInDirection(direction);
        int cost = GetMovementCost(tileFrom, tileTo);
        if (!Player.HasEnoughActionPoints(cost)) {
            Debug.LogWarning("Cannot move because not enough AP");
            return false;
        }

        // change the player's position.
        Player.SetLocation(tileTo.CellLoc, direction);

        // Bookkeeping
        Player.SpendActionPoints(cost);

        // reveal tile and neighbors
        // TODO: refactor this into a worldMap class that contains and manages the tiless
        var tilesChanged = tileTo.Reveal(1);
        // this would be a "on tiles changed event" that worldMapLoader would subscribe too
        WorldMapLoader.Instance.LoadWorldTileListIntoTileMap(tilesChanged);

        return true;
    }

    // ACTIONS

    public bool Camp()
    {
        if (!IsAllowedOnPlayerTile(ActionType.Camp)) { return false; }

        const ItemId ration = ItemId.Ration;
        bool hasFood = Inventory.HasItem(ration);
        if (hasFood)
        {
            Debug.Log("Eating a ration");
            Inventory.RemoveItem(ration);
        }
        else
        {
            Debug.Log("Health loss due to no food");
            Player.ReduceHealth();
        }
        // give back AP
        Player.ResetActionPoints();
        // advance date
        GameStateMeta.AddDays(1);
        return true;
    }

    public int GetForageCost(){ return 4; }

    public bool CanForage(){ return CanForage(out _); }

    public bool CanForage(out int chance)
    {
        return IsAllowedOnTileAndHaveAP(ActionType.Forage, GetForageCost(), out chance);
    }

    public bool Forage()
    {
        int cost = GetForageCost();
        if (!CanForage(out int chance)){ return false; }

        return TakeActionForItem(cost, chance, ItemId.Ration, 1);
    }

    public int GetPanForGoldCost() { return 2; }

    public bool CanPanForGold() { return CanPanForGold(out _); }

    public bool CanPanForGold(out int chance)
    {
        chance = 0; // set in case hasPan is false
        bool hasPan = Inventory.HasItem(ItemId.Pan);       
        return hasPan && IsAllowedOnTileAndHaveAP(ActionType.PanForGold, GetPanForGoldCost(), out chance);
    }

    public bool PanForGold()
    {
        int cost = GetPanForGoldCost();
        if (!CanPanForGold(out int chance)) { return false; }

        return TakeActionForItem(cost, chance, ItemId.GoldNugget, 1);
    }

    public bool IsAllowedOnTileAndHaveAP(ActionType type, int cost, out int chance)
    {
        return IsAllowedOnPlayerTile(type, out chance) && Player.HasEnoughActionPoints(cost);
    }

    public bool IsAllowedOnTileAndHaveAP(ActionType type, int cost)
    {
        return IsAllowedOnPlayerTile(type, out _) && Player.HasEnoughActionPoints(cost);
    }

    public bool IsAllowedOnPlayerTile(ActionType type)
    {
        return IsAllowedOnPlayerTile(type, out _);
    }

    public bool IsAllowedOnPlayerTile(ActionType type, out int chance)
    {
        chance = 0;
        switch (type)
        {
            case ActionType.Camp:
                return GetTileForPlayerLocation().CanCamp;
            case ActionType.Forage:
                chance = GetForageChanceForPlayerTile();
                return chance > 0;
            case ActionType.Hunt:
                chance = GetHuntingChanceForPlayerTile();
                return chance > 0;
            case ActionType.PanForGold:
                chance = GetGoldPanningChanceForPlayerTile();
                return chance > 0;
            default:
                return true;
        }
    }

    public int GetForageChanceForPlayerTile()
    {
        WorldTile tileAt = GetTileForPlayerLocation();
        return tileAt.ForagingChance;
    }

    public int GetHuntingChanceForPlayerTile()
    {
        WorldTile tileAt = GetTileForPlayerLocation();
        return tileAt.HuntingChance;
    }

    public int GetGoldPanningChanceForPlayerTile()
    {
        WorldTile tileAt = GetTileForPlayerLocation();
        if (tileAt.HasRiver()) {
            // TODO: this should probably be based on the tile or something
            return 20;
        }
        return 0;
    }

    private WorldTile GetTileForPlayerLocation()
    {
        return gameState.GetTileForPlayerLocation(Player);
    }

    /// <summary>
    /// Take an action that has a chance to yield item
    /// </summary>
    /// <param name="cost">AP cost to take the action</param>
    /// <param name="chance">chance (/100) to get succeed</param>
    /// <param name="type">type of item if success</param>
    /// <param name="quantity">amount of item if success</param>
    /// <returns>success rv</returns>
    private bool TakeActionForItem(int cost, int chance, ItemId type, int quantity = 1)
    {
        if (!Player.SpendActionPoints(cost)) { return false; }

        bool success = RollDice(chance);
        if (success)
        {
            Inventory.AddItem(type, quantity);
        }
        return success;
    }

    /// <summary>
    /// Roll dice to see if a chance action has succeeded or not    
    /// </summary>
    /// <param name="chance">int chance / 100 e.x. 25 = 25% chance</param>
    /// <returns></returns>
    private bool RollDice(int chance)
    {
        int roll = UnityEngine.Random.Range(0, 99);
        bool success = chance >= roll;
        return success;
    }

    // SHOP

    public bool CanAfford(int cost)
    {
        return Player.HasEnoughMoney(cost);
    }

    public bool BuyItem(ItemId type, int cost, int amount = 1)
    {
        var totalCost = cost * amount;
        if (!Player.SpendMoney(cost)) { return false; }
            
        Inventory.AddItem(type, 1);
        return true;
    }

    public bool CanSell(ItemId type)
    {
        return GetSellPrice(type) > 0;
    }

    public int GetSellPrice(ItemId type)
    {
        ItemData data = ItemDataLoader.LoadItemById(type);
        if (data.price > 0)
        {
            float priceRaw = data.price / 2;
            int price = (priceRaw > 1) ? (int) Math.Floor(priceRaw) : 1;
            return price;
        }
        return -1;
    }

    public bool SellItem(ItemId type, int price, int amount = 1)
    {
        if (!Inventory.RemoveItem(type, amount)) { return false; }

        Player.ReceiveMoney(amount * price);

        return true;
    }

    // WORLD

    // gets the store inventory.
    public Inventory GetStoreInventory()
    {
        Inventory inventory = new Inventory();
        // TODO: should this binding live in SO?
        var categories = new List<ItemCategory>( new[]{ ItemCategory.Food, ItemCategory.Tools });

        // Look at each item and determine if it fits
        foreach (ItemId type in Enum.GetValues(typeof(ItemId)))
        {
            var item = new InventoryItem(type, 10);
            // needs to have a price
            if (!(item.Price > 0)) { continue; }
            // needs to be a matching category
            if (!(categories.Contains(item.Category))){ continue; }
            inventory.AddItem(item);
        }

        return inventory;
    }
}
