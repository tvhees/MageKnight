using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Other.Factory;
using Other.Data;
using Other.Utility;

public class Board : NetworkBehaviour
{
    public BoardFactory boardFactory;
    public GameObject holderPrefab;

    private List<HexVector> tilePositions;
    private List<GameObject> countrysideTiles;
    private List<GameObject> coreTiles;

    private GameObject tileStack;
    private GameObject boardHolder;

    #region Public
    public void CreateBoard(Scenario scenario)
    {
        DataForPlayerCount data = scenario.playerCounts[Network.connections.Length + 1 - scenario.minPlayers];

        CreateTilesForScenario(data);
        CreateStartingBoard(data);
    }
    #endregion

    #region Private

    private void CreateTilesForScenario(DataForPlayerCount data)
    {
        tileStack = Instantiate(holderPrefab);
        tileStack.name = "Tile Stack";
        NetworkServer.Spawn(tileStack);

        countrysideTiles = boardFactory.CreateCountrysideStack(data);
        coreTiles = boardFactory.CreateCoreAndCityStack(data);

        AddTilesToStack(countrysideTiles);
        AddTilesToStack(coreTiles);

        tileStack.transform.position = new Vector3(-30f, 30f, 0f);
    }

    private void AddTilesToStack(List<GameObject> tileList)
    {
        foreach (GameObject tile in tileList)
            tile.transform.SetParent(tileStack.transform);
    }

    private void CreateStartingBoard(DataForPlayerCount playerCountData)
    {
        boardHolder = Instantiate(holderPrefab);
        boardHolder.name = "Board Holder";
        NetworkServer.Spawn(boardHolder);

        var mapShape = MapShapeDatabase.GetScriptableObject(playerCountData.shape.ToString());
        tilePositions = new List<HexVector>(mapShape.listOfTilePositions);

        AddPortalTileToBoard(mapShape);
        AddStartingCountryside(mapShape);
    }

    private void AddPortalTileToBoard(MapShape mapShape)
    {
        GameObject portalTile = boardFactory.CreateStartTile(mapShape);
        portalTile.transform.SetParent(boardHolder.transform);
        tilePositions.RemoveAt(0);

        portalTile.transform.GetChild(3).tag = "PortalTile";
    }

    private void AddStartingCountryside(MapShape mapShape)
    {
        for (int i = 1; i < mapShape.startingCountryside; i++)
        {
            PlaceNewTile();
        }
    }

    private void PlaceNewTile()
    {
        GameObject tile = countrysideTiles[0];
        tile.transform.SetParent(boardHolder.transform);
        tile.transform.localPosition = tilePositions[0].worldVector;

        countrysideTiles.RemoveAt(0);
        tilePositions.RemoveAt(0);
    }
    #endregion
}