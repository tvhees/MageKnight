using UnityEngine;
using System.Collections;
using Other.Data;
using Other.Utility;

public struct HexId
{
    public Vector3 localPosition;
    public GameConstants.TerrainType terrain;
    public GameConstants.FeatureType feature;

    public HexId(HexTile hexTile, int hexIndex)
    {
        localPosition = hexTile.localCoordinates[hexIndex].worldVector;
        terrain = hexTile.hexes[hexIndex];
        feature = hexTile.features[hexIndex];
    }
}
