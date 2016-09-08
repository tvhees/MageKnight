using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class BoardSetup : NetworkBehaviour {

    public GameObject holderPrefab;

    public StateController stateController;
    public BoardFactory boardFactory;
    public Board board;

    [ServerCallback]
    void OnEnable()
    {
        CreateBoard(GameController.singleton.scenario);
    }

    public void CreateBoard(Scenario scenario)
    {
        DataForPlayerCount data = scenario.playerCounts[Network.connections.Length + 1 - scenario.minPlayers];

        CreateTilesForScenario(data);
        CreateStartingBoard(data);
    }

    #region Private
    private void CreateTilesForScenario(DataForPlayerCount data)
    {
        GameObject tileStack = Instantiate(holderPrefab);
        tileStack.name = "Tile Stack";
        stateController.ServerSpawnObject(tileStack);

        List<GameObject> countrysideTiles = boardFactory.CreateCountrysideStack(data);
        List<GameObject> coreTiles = boardFactory.CreateCoreAndCityStack(data);

        AddTilesToStack(countrysideTiles, tileStack);
        AddTilesToStack(coreTiles, tileStack);

        tileStack.transform.position = new Vector3(-30f, 30f, 0f);
        board.tileStack = tileStack;
    }

    private void AddTilesToStack(List<GameObject> tileList, GameObject tileStack)
    {
        foreach (GameObject tile in tileList)
            tile.transform.SetParent(tileStack.transform);
    }

    private void CreateStartingBoard(DataForPlayerCount playerCountData)
    {
        var mapShape = MapShapeDatabase.GetScriptableObject(playerCountData.shape.ToString());
        board.tilePositions = new List<HexVector>(mapShape.listOfTilePositions);

        AddPortalTileToBoard(mapShape);
        AddStartingCountryside(mapShape);
    }

    private void AddPortalTileToBoard(MapShape mapShape)
    {
        GameObject portalTile = boardFactory.CreateStartTile(mapShape);
        portalTile.transform.SetParent(board.transform);
        board.tilePositions.RemoveAt(0);

        portalTile.transform.GetChild(3).tag = "PortalTile";
    }

    private void AddStartingCountryside(MapShape mapShape)
    {
        for (int i = 1; i < mapShape.startingCountryside; i++)
        {
            board.PlaceNewTile();
        }
    }
    #endregion
}
