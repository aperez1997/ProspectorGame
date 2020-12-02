using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Loads TileFeaturedData SO objects
/// </summary>
public class TileFeatureDataLoader : AssetLoader<TileFeatureData>
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

    public static TileFeatureData LoadByType_Static(TileFeatureType type)
    {
        return Instance.LoadByKey(type.ToString());
    }
}