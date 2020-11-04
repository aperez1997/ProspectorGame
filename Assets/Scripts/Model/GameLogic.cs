using System;
using UnityEngine;

public class GameLogic
{
    private Player Player { get { return gameState.Player; } }

    private Inventory Inventory { get { return Player.Inventory; } }

    private readonly GameState gameState;

    public GameLogic(GameState gameState)
    {
        this.gameState = gameState;
    }

    // ACTIONS

    public bool Camp()
    {
        if (!IsAllowedOnPlayerTile(ActionType.Camp)) { return false; }

        const ItemType ration = ItemType.Ration;
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
        Player.ResetActionPoints();
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

        return TakeActionForItem(cost, chance, ItemType.Ration, 1);
    }

    public int GetPanForGoldCost() { return 2; }

    public bool CanPanForGold() { return CanPanForGold(out _); }

    public bool CanPanForGold(out int chance)
    {
        chance = 0;
        bool hasPan = Inventory.HasItem(ItemType.Pan);       
        return hasPan && IsAllowedOnTileAndHaveAP(ActionType.Pan, 0, out chance);
    }

    public bool PanForGold()
    {
        int cost = GetPanForGoldCost();
        if (!CanPanForGold(out int chance)) { return false; }

        return TakeActionForItem(cost, chance, ItemType.GoldNugget, 1);
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
            case ActionType.Pan:
                // TODO: check for rivers
                return false;
            default:
                return true;
        }
    }

    public int GetForageChanceForPlayerTile()
    {
        DataTile dataTileAt = GetTileForPlayerLocation();
        return dataTileAt.ForagingChance;
    }

    public int GetHuntingChanceForPlayerTile()
    {
        DataTile dataTileAt = GetTileForPlayerLocation();
        return dataTileAt.HuntingChance;
    }

    private DataTile GetTileForPlayerLocation()
    {
        return gameState.GetTileForPlayerLocation(Player);
    }

    private bool TakeActionForItem(int cost, int chance, ItemType type, int quantity = 1)
    {
        if (!Player.SpendActionPoints(cost)) { return false; }

        bool success = RollDice(chance);
        if (success)
        {
            Inventory.AddItem(type, quantity);
        }
        return success;
    }

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

    public bool BuyItem(ItemType type, int cost, int amount = 1)
    {
        var totalCost = cost * amount;
        if (!Player.SpendMoney(cost)) { return false; }
            
        Inventory.AddItem(type, 1);
        return true;
    }

    public bool CanSell(ItemType type)
    {
        return GetSellPrice(type) > 0;
    }

    public int GetSellPrice(ItemType type)
    {
        ItemData data = ItemDataLoader.LoadItemByType(type);
        if (data.price > 0)
        {
            float priceRaw = data.price / 2;
            int price = (priceRaw > 1) ? (int) Math.Floor(priceRaw) : 1;
            return price;
        }
        return -1;
    }

    public bool SellItem(ItemType type, int price, int amount = 1)
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

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            var item = new InventoryItem(type, 10);
            if (!(item.Price > 0)) { continue; }
            inventory.AddItem(item);
        }

        return inventory;
    }
}
