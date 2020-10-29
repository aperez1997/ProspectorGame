﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeDataLoader : MonoBehaviour
{
    public static BiomeDataLoader Instance;

    private readonly Dictionary<BiomeType, BiomeData> DataDict = new Dictionary<BiomeType, BiomeData>();

    private void Awake()
    {
        Instance = this;
        PopulateList();
    }

    public static BiomeData LoadBiomeDataByType(BiomeType type)
    {
        return Instance.LoadByType(type);
    }

    public BiomeData LoadByType(BiomeType type)
    {
        bool rv = DataDict.TryGetValue(type, out BiomeData item);
        if (!rv) { Debug.LogError("No BiomeData SO found for type [" + type + "]"); }
        return item;
    }

    private void PopulateList()
    {
        DataDict.Clear();
        string[] assetNames = AssetDatabase.FindAssets("t:BiomeData", new[] { "Assets/GameData/Biomes" });
        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<BiomeData>(SOPath);
            DataDict.Add(item.type, item);
        }
        Debug.Log("Found " + DataDict.Count + " BiomeDatums");
    }
}