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
}
