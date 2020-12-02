using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileFeature", menuName = "TileFeatureData")]
public class TileFeatureData : ScriptableObject, IAssetLoaderItem
{
    public TileFeatureType type;

    public new string name;

    public string description;

    public Tile hexTile;

    [Tooltip("Which z-value is used in the tilemap")]
    public int zIndex;

    [Tooltip("Modifies base move cost")]
    public int moveCostModifier;

    [Tooltip("Modifies base foraging chance")]
    public int foragingChanceModifier;

    [Tooltip("Modifies base hunting chance")]
    public int huntingChanceModifier;

    public string GetKey() { return type.ToString(); }
}

// Note: this currently map to which tilemap layer the features go on.
public enum TileFeatureType { River = 1, Road = 2, Town = 3, }