using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Loads BiomeData stored objects
/// </summary>
[CreateAssetMenu(fileName = "Biome", menuName = "BiomeData")]
public class BiomeData : ScriptableObject, IAssetLoaderItem
{
    public BiomeType type;

    public new string name;

    public string description;

    public Tile hexTile;

    [Tooltip("Number of AP to move to this tile")]
    public int moveBaseCost;

    [Tooltip("true if you can camp here")]
    public bool canCamp;

    [Tooltip("If set, foraging chance / 100")]
    public int foragingBaseChance;

    [Tooltip("If set, Hunting chance / 100")]
    public int huntingBaseChance;

    public static BiomeType GetRandomTypeForRandoMap()
    {
        System.Array values = System.Enum.GetValues(typeof(BiomeType));
        int index = Random.Range(1, values.Length); // don't return water tiles
        return (BiomeType)values.GetValue(index);
    }

    public string GetKey(){ return type.ToString(); }
}

// All the known biome types. Types are spelled out because they get saved into the saveFile
public enum BiomeType { Water = 1, Grass = 2, Forest = 3, Hills = 4, Mountains = 5 };
