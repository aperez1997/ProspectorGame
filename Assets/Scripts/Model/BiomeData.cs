using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Holds data about the different types of biomes, as well as it's asset for the map
/// </summary>
[CreateAssetMenu(fileName = "Biome", menuName = "BiomeData")]
public class BiomeData : ScriptableObject
{
    public BiomeType type;

    public new string name;

    public string description;

    public Tile hexTile;

    [Tooltip("Number of AP to move to this tile")]
    public int baseMoveCost;

    [Tooltip("true if you can camp here")]
    public bool canCamp;

    [Tooltip("If set, foraging chance / 100")]
    public int foragingBaseChance;

    [Tooltip("If set, Hunting chance / 100")]
    public int huntingBaseChance;

    public static BiomeType GetRandomTypeForRandoMap()
    {
        System.Array values = System.Enum.GetValues(typeof(BiomeType));
        int index = Random.Range(0, values.Length - 1); // Don't return town tiles
        return (BiomeType)values.GetValue(index);
    }
}

// All the known biome types. Types are spelled out because they get saved into the saveFile
public enum BiomeType { Water = 1, Grass = 2, Forest = 3, Hills = 4, Mountains = 5, Town = 6 };
