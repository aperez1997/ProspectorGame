using UnityEngine;
using UnityEngine.Tilemaps;

public class TileAssets : MonoBehaviour
{
    public static TileAssets Instance { get; private set; }

    public Tile waterTile;
    public Tile grassTile;
    public Tile forestTile;
    public Tile hillsTile;
    public Tile mountainTile;
    public Tile townTile;

    private void Awake()
    {
        Instance = this;
    }
}
