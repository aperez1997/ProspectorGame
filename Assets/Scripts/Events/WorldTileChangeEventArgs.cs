using System;
using System.Collections.Generic;

/// <summary>
/// When world tiles are changed
/// </summary>
public class WorldTileChangeEventArgs : EventArgs
{
    public List<WorldTile> WorldTileList { get; }

    public WorldTileChangeEventArgs(List<WorldTile> worldTilesChanged)
    {
        WorldTileList = worldTilesChanged;
    }
}
