using Other.Factory;
using Other.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardView : NetworkBehaviour
{
    public HexFactory hexFactory;

    public List<HexVector> tilePositions;

    public GameObject tileStack;

    [ClientRpc]
    public void RpcCreateTile(TileId tileId)
    {
        var tile = new GameObject("Tile");
        tile.transform.position = tileId.position;
        foreach (var hexId in tileId.hexes)
        {
            var hex = hexFactory.CreateSceneObject(hexId);
            hex.transform.SetParent(tile.transform);
            hex.transform.position = hexId.position;
        }
    }
}