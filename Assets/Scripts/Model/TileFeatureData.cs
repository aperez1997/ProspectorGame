using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileFeature", menuName = "TileFeatureData")]
public class TileFeatureData : ScriptableObject
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
}

// Note: this currently map to which tilemap layer the features go on.
public enum TileFeatureType { River = 1, Road = 2, Town = 3, }

// TODO: Move to own file
// TODO: Can this templated? its basically copied from BiomeDataLoader
public class TileFeatureDataLoader
{
    private static TileFeatureDataLoader instance = null;
    private static readonly object padlock = new object();

    public static TileFeatureDataLoader Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null) { instance = new TileFeatureDataLoader(); }
                return instance;
            }
        }
    }

    private readonly Dictionary<TileFeatureType, TileFeatureData> DataDict = new Dictionary<TileFeatureType, TileFeatureData>();

    public TileFeatureDataLoader()
    {
        PopulateList();
    }

    public static TileFeatureData LoadByType_Static(TileFeatureType type)
    {
        return Instance.LoadByType(type);
    }

    public TileFeatureData LoadByType(TileFeatureType type)
    {
        bool rv = DataDict.TryGetValue(type, out TileFeatureData item);
        if (!rv) { Debug.LogError("No TileFeatureData SO found for type [" + type + "]"); }
        return item;
    }

    private void PopulateList()
    {
        DataDict.Clear();
        string[] assetNames = AssetDatabase.FindAssets("t:TileFeatureData", new[] { "Assets/GameData/Features" });
        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<TileFeatureData>(SOPath);
            DataDict.Add(item.type, item);
        }
        Debug.Log("Found " + DataDict.Count + " TileFeatureDatums");
    }
}