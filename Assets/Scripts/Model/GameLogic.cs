using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Has all the game logic, as well as the game state
/// TODO: I need to create a monobehavior singleton (like tooltipv2) that can bind SOs for new game stuff
/// This will allow me to bind "well rested" and grant to player at start
/// </summary>
public class GameLogic
{
    public readonly GameState GameState;
    public GameStateMeta GameStateMeta { get { return GameState.GameStateMeta; } }

    public Player Player { get { return GameState.Player; } }

    public Inventory Inventory { get { return Player.Inventory; } }

    public WorldMap WorldMap { get { return GameState.WorldMap; } }

    private readonly ScriptedObjectBinder soBinder;

    /// <summary>
    /// Used when creating a new game, as the gamestate will be created.
    /// </summary>
    public GameLogic(ScriptedObjectBinder ScriptedObjectBinder)
    {
        soBinder = ScriptedObjectBinder;

        Debug.Log("Creating player...");
        Player player = new Player();

        List<WorldTile> TileList = CreateRandomWorldMap();
        var WorldMap = new WorldMap(TileList);

        Debug.Log("Creating game state meta...");
        var meta = new GameStateMeta();
        this.GameState = new GameState(meta, player, WorldMap);
    }

    /// <summary>
    /// used when loading a game
    /// </summary>
    public GameLogic(GameState gameState, ScriptedObjectBinder ScriptedObjectBinder)
    {
        soBinder = ScriptedObjectBinder;
        this.GameState = gameState;
    }

    public void InitNewGame()
    {
        // Player starting EQ
        Debug.Log("Adding player starting EQ");
        Player.Inventory.AddItem(ItemId.Money, 50);
        Player.Inventory.AddItem(ItemId.Ration, 7);
        Player.Inventory.AddItem(ItemId.Pan, 1);
        Player.Status.AddEffect(soBinder.seWellRested);
        Player.ActionPoints = GetPlayerMaxActionPoints();

        // reveal players location
        Debug.Log("Revealing player location...");
        var tileAt = GetTileForPlayerLocation();
        tileAt.Reveal(1);
    }

    public List<WorldTile> CreateRandomWorldMap()
    {
        Debug.Log("Creating new random world");

        /**
         * consider using data-struct (this many mountains, this height of water)
         * that can be made into a hash, and hash always generates the same level
         */

        List<WorldTile> dTileList = new List<WorldTile>();

        int maxX = 5;
        int maxY = 5;

        // generates a random map for now
        for (int x = -1 * maxX; x <= maxX; x++) {
            for (int y = -1 * maxY; y <= maxY; y++) {
                Vector3Int loc = new Vector3Int(x, y, 0);

                BiomeType tt = BiomeData.GetRandomTypeForRandoMap();
                // surround the edges of the map with water
                if (Math.Abs(x) == maxX || Math.Abs(y) == maxY) { tt = BiomeType.Water; }

                // grass around the center
                if (Math.Abs(x) <= 1 && Math.Abs(y) <= 1) { tt = BiomeType.Grass; }

                var goldRichness = WorldTile.GetRandomRichness();
                WorldTile worldTile = new WorldTile(loc, tt, goldRichness);
                dTileList.Add(worldTile);

                // add features
                if (x == 0 && y == 0) {
                    // always start in town
                    worldTile.AddFeature(TileFeatureType.Town);
                }
                if (RoadLocs.Contains((x, y))) {
                    // Road locations
                    worldTile.AddFeature(TileFeatureType.Road);
                }

                if (RiverLocs.Contains((x, y))) {
                    // Road locations
                    worldTile.AddFeature(TileFeatureType.River);
                }
            }
        }
        return dTileList;
    }

    private static List<(int, int)> RoadLocs = new List<(int, int)>(new (int, int)[]{
        (-1,-1), (0,0), (1,0), (1,1), (1,2), (1,-1)
    });

    private static List<(int, int)> RiverLocs = new List<(int, int)>(new (int, int)[]{
        (-4,-4), (-4,-3), (-4,-2), (-4,-1), (-3,0), (-3,1)
    });

    // Movement

    /// <summary>
    /// Gets AP cost to move from tileAt to tileNeighbor
    /// </summary>
    public int GetMovementCost(WorldTile tileAt, WorldTile tileNeighbor)
    {
        bool hasRoad = tileAt.HasRoad();
        int costNeighbor = tileNeighbor.GetMoveCost(hasRoad);
        if (tileNeighbor.MoveBaseCost > 0){
            // Only enforce min-cost-1 if the tile is moveable (negative base cost means can't move)
            costNeighbor = Math.Max(1, costNeighbor);
        }
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

        // reveal the tile we moved to
        WorldMap.RevealTile(tileTo);

        return true;
    }

    // ACTIONS

    public bool Camp()
    {
        if (!IsAllowedOnPlayerTile(ActionType.Camp)) { return false; }

        var food = GetAllPlayerFood();
        InventoryItem foodToEat = food.FirstOrDefault();

        if (foodToEat is InventoryItem) {
            Debug.Log("Eating some food");
            Inventory.RemoveItem(foodToEat.id, 1);
        } else {
            Debug.Log("Health loss due to no food");
            Player.ReduceHealth();
        }

        // give back AP
        Player.ActionPoints = GetPlayerMaxActionPoints();
        // advance date
        GameStateMeta.AddDays(1);

        // advance day for player status effects
        var expireList = new List<PlayerStatusEffect>();
        foreach (var psEffect in Player.Status.EffectsList) {
            psEffect.DaysLeft -= 1;
            if (psEffect.DaysLeft < 0) {
                // remove expired effect
                expireList.Add(psEffect);
            }
        }
        Player.Status.RemoveEffectList(expireList);

        return true;
    }

    public int GetPlayerMaxActionPoints()
    {
        // AP reduced by health loss.
        int healthLossAPRedux = -1 * (Player.MAX_HEALTH - Player.Health);

        // effect loss
        int totalEffectChange = Player.Status.GetTotalStatEffect(PlayerStat.ActionPoints);

        Debug.Log("AP Max:" + Player.ActionPointsMax + " effects:" + totalEffectChange + " health loss:" + healthLossAPRedux);
        // don't go below 1
        return Math.Max(1, Player.ActionPointsMax + totalEffectChange - healthLossAPRedux);
    }


    /// <summary>
    /// Find all the player's food items, sorted by lowest price first
    /// </summary>
    private List<InventoryItem> GetAllPlayerFood()
    {
        // find all food items
        var items = Inventory.GetItemsByCategory(ItemCategory.Food);

        // sort by price
        items.Sort(delegate (InventoryItem x, InventoryItem y) {
            return x.Price.CompareTo(y.Price);
        });

        return items;
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

        return TakeActionForItem(cost, chance, ItemId.ForagedFood, 1);
    }

    public int GetHuntCost() { return 2; }

    public bool CanHunt() { return CanHunt(out _, out _); }

    public bool CanHunt(out int chance) { return CanHunt(out chance, out _); }

    public bool CanHunt(out int chance, out InventoryItem bestWeapon)
    {
        // Weapon check is buried in here
        var rv = IsAllowedOnTileAndHaveAP(ActionType.Hunt, GetHuntCost(), out chance);

        // look for the best working weapon (has ammo)
        bestWeapon = GetBestWorkingWeapon();
        if (bestWeapon is InventoryItem actualWeapon) {
            chance += actualWeapon.HuntingModifier ?? 0;
        } else {
            // no weapon = no chance
            chance = 0;
            rv = false;
        }

        return rv;
    }

    public bool Hunt()
    {
        int cost = GetHuntCost();
        if (!CanHunt(out var chance, out var bestWeapon)) { return false; }

        var weaponStr = bestWeapon == null ? "NULL!" : bestWeapon.ToString();
        Debug.Log("Hunting with " + weaponStr);

        bool rv = TakeActionForItem(cost, chance, ItemId.ForagedFood, 1);
        // chance to lose ammo even if you don't catch something
        bool usedAmmoAnyway = RollDice(25);

        // if we caught something or used ammo anyway, deduct ammo
        if (rv || usedAmmoAnyway) {
            if (bestWeapon is InventoryItem && bestWeapon.AmmoId is ItemId ammoId) {
                Debug.Log("Removed ammo because " + (rv ? "success" : "failed but used ammo"));
                Inventory.RemoveItem(ammoId);
            } else {
                var debug = bestWeapon == null ? "null weapon" : bestWeapon.ToString();
                Debug.LogWarning("Hunted but weapon is missing or missing ammo? " + debug);
            }
        }

        return rv;
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
                chance = GetGoldPanningChanceForPlayerTile(out var hasRiver);
                return hasRiver;
            default:
                return true;
        }
    }

    public InventoryItem GetBestWorkingWeapon()
    {
        var weapons = Inventory.GetItemsByCategory(ItemCategory.Weapons);
        // use linq to get only working weapons
        var workingWeapons = from weapon in weapons
                             where Inventory.HasAmmoForWeapon(weapon)
                             select weapon;
        // use linq to sort by hunting modifier (asc)
        workingWeapons.OrderBy(item => item.HuntingModifier);
        return workingWeapons.LastOrDefault();
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

    public int GetGoldPanningChanceForPlayerTile(out bool hasRiver)
    {
        WorldTile tileAt = GetTileForPlayerLocation();
        hasRiver = tileAt.HasRiver();
        if (hasRiver) {
            // chance depends on richness
            switch (tileAt.GoldRichness) {
                case Richness.High:
                    return 25;
                case Richness.Medium:
                    return 15;
                case Richness.Low:
                    return 5;
            }
        }
        return 0;
    }

    public WorldTile GetTileForPlayerLocation()
    {
        return GameState.GetTileForPlayerLocation();
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
        var categories = new List<ItemCategory>(
            new[]{ ItemCategory.Food, ItemCategory.Tools, ItemCategory.Weapons, ItemCategory.Ammo }
        );

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
