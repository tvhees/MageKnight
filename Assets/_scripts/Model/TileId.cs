using UnityEngine;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;

public struct TileId
{
    public HexId[] hexes;
    public Vector3 position;

    public TileId(HexId[] hexes, Vector3 position)
    {
        this.hexes = hexes;
        this.position = position;
    }

    public void SetTilePosition(Vector3 position)
    {
        this.position = position;
        foreach (var hex in hexes)
            hex.SetNewPosition(position);
    }
}
