using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A single tile in the serialized world map
/// </summary>
[Serializable]
public class WorldTile : ISerializationCallbackReceiver
{
    /// <summary> Location (x,y) in the world map</summary>
    public Vector3Int CellLoc;

    /// <summary> Type of Biome </summary>
    public BiomeType Type;

    /// <summary>Lazy-Loaded BiomeData object</summary>
    private BiomeData _biomeData;
    public BiomeData BiomeData {
        get {
            if (_biomeData is null) {
                var data = BiomeDataLoader.LoadBiomeDataByType(Type);
                if (!(data is BiomeData)) { 
                    throw new Exception("No biome data for type " + Type.ToString());
                }
                _biomeData = data;
            }
            return _biomeData;
        }
    }

    // Derived field that come from BiomeData
    /// <summary>Sprite asset for the tile</summary>
    public Tile Tile { get { return BiomeData.hexTile; } }
    public bool CanCamp { get { return BiomeData.canCamp; } }
    private int MoveBaseCost { get { return BiomeData.moveBaseCost; } }
    private int ForagingBaseChance { get { return BiomeData.foragingBaseChance; } }
    private int HuntingBaseChance { get { return BiomeData.huntingBaseChance; } }

    /// <summary>Identifies which features exist on this tile</summary>
    [SerializeField] private List<TileFeatureType> FeatureTypes;

    /// <summary>Lazy-loaded feature SO data </summary>
    private List<TileFeatureData> _features = null;
    public List<TileFeatureData> Features {
        get {
            if (_features == null) {
                _features = new List<TileFeatureData>();
                foreach (var featureType in FeatureTypes){
                    _features.Add(TileFeatureDataLoader.LoadByType_Static(featureType));
                }
            }
            return _features;
        }
    }

    /// <summary>
    /// computed foraging chance
    /// </summary>
    public int ForagingChance {
        get {
            var chance = ForagingBaseChance;
            foreach (var feature in Features) {
                chance += feature.foragingChanceModifier;
            }
            return chance;
        }
    }

    /// <summary>
    /// Computed hunting chance
    /// </summary>
    public int HuntingChance {
        get {
            var chance = HuntingBaseChance;
            foreach (var feature in Features) {
                chance += feature.huntingChanceModifier;
            }
            return chance;
        }
    }

    /// <summary>
    /// Hash of Direction->WorldTile, the neighbors around this tile
    /// </summary>
    public Dictionary<HexDirection, WorldTile> Neighbors { get; set; }

    public WorldTile(Vector3Int cellLocationIn, BiomeType type)
    {
        CellLoc = cellLocationIn;
        Type = type;
        FeatureTypes = new List<TileFeatureType>();
        ResetLoadedFields();
    }

    private void ResetLoadedFields()
    {
        _biomeData = null;
    }

    public bool HasFeature(TileFeatureType type)
    {
        return FeatureTypes.Contains(type);
    }

    public bool HasRoad()
    {
        return HasFeature(TileFeatureType.Road);
    }

    public void AddFeature(TileFeatureType type)
    {
        if (!HasFeature(type)){
            FeatureTypes.Add(type);
            ResetLoadedFields(); // need to recompute
        }
    }

    /// <summary>
    /// Computed move cost. Has to be function because it's conditional on the
    /// road bonus (which is tile dependent)
    /// </summary>
    public int GetMoveCost(bool roadBonus)
    {
        var moveCost = MoveBaseCost;
        foreach (var feature in Features) {
            if (roadBonus || feature.type != TileFeatureType.Road) {
                moveCost += feature.moveCostModifier;
            }
        }
        
        return moveCost;
    }

    public override string ToString()
    {
        return "DT[loc" + CellLoc.ToString() + "t(" + Type.ToString() + "]";
    }

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize()
    {
        // need to do this, otherwise the loaded fields wont be in a good state
        ResetLoadedFields();
    }
}
