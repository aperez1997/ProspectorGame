using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Biome", menuName = "BiomeData")]
public class BiomeData : ScriptableObject
{
    public BiomeType type;

    public new string name;

    public string description;

    public Tile hexTile;

    public int baseMoveCost;

    public static BiomeType GetRandomTypeForRandoMap()
    {
        System.Array values = System.Enum.GetValues(typeof(BiomeType));
        int index = Random.Range(0, values.Length - 1); // Don't return town tiles
        return (BiomeType)values.GetValue(index);
    }
}

// All the known biome types. Types are spelled out because they get saved into the saveFile
public enum BiomeType { Water = 1, Grass = 2, Forest = 3, Hills = 4, Mountains = 5, Town = 6 };
