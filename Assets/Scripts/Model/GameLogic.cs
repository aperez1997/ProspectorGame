using System;
using UnityEngine;

public class GameLogic
{
    public Player Player { get { return gameState.Player; } }

    public Inventory Inventory { get { return Player.Inventory; } }

    private readonly GameState gameState;

    public GameLogic(GameState gameState)
    {
        this.gameState = gameState;
    }

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
            Player.Health -= 1;
        }
        Player.ResetActionPoints();
        return true;
    }

    public int GetForageCost(){ return 6; }

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
        Player.ActionPoints -= cost;

        bool success = RollDice(chance);
        if (success)
        {
            Inventory.AddItem(new InventoryItem(type, quantity));
        }
        return success;
    }

    private bool RollDice(int chance)
    {
        int roll = UnityEngine.Random.Range(0, 99);
        bool success = chance >= roll;
        return success;
    }
}
