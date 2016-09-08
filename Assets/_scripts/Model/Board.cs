using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Other.Factory;
using Other.Data;
using Other.Utility;

public class Board : NetworkBehaviour
{
    public List<HexVector> tilePositions;

    public GameObject tileStack;

    public void PlaceNewTile()
    {
        GameObject tile = tileStack.transform.GetChild(0).gameObject;
        tile.transform.SetParent(transform);
        tile.transform.localPosition = tilePositions[0].worldVector;
        tilePositions.RemoveAt(0);
    }
}