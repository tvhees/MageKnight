using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Other.Factory;
using Other.Data;
using Other.Utility;

public class Board : NetworkBehaviour
{
    public List<HexVector> tilePositions;
    public NetworkHeirarchySync boardHeirarchy;

    public GameObject tileStack;

    [Server]
    public void ServerPlaceNewTile()
    {
        GameObject tile = tileStack.transform.GetChild(0).gameObject;
        tile.transform.SetParent(transform);
        boardHeirarchy.ServerSyncChild(tile);
        tile.transform.localPosition = tilePositions[0].worldVector;
        tilePositions.RemoveAt(0);
    }
}