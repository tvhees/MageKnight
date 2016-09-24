using UnityEngine;
using System.Collections;
using Other.Data;
using Other.Utility;

public struct HexId
{
    public Vector3 position;
    private Vector3 localPosition;
    public GameConstants.TerrainType terrain;
    public GameConstants.FeatureType feature;

    public HexId(HexTile hexTile, int hexIndex, Vector3 tilePosition = default(Vector3))
    {
        position = Vector3.zero;
        localPosition = hexTile.localCoordinates[hexIndex].worldVector;
        terrain = hexTile.hexes[hexIndex];
        feature = hexTile.features[hexIndex];

        SetNewPosition(tilePosition);
    }

    public void SetNewPosition(Vector3 tilePosition)
    {
        position = tilePosition + localPosition;
    }

    public bool isTraversable
    {
        get { return GameController.singleton.movementCosts.IsTraversable(terrain); }
    }

    public int movementCost
    {
        get { return GameController.singleton.movementCosts.MoveCost(terrain); }
    }
}
