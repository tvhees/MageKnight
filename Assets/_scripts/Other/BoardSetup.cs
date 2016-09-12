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
        ServerCreateBoard(GameController.singleton.scenario);
    }

    [Server]
    public void ServerCreateBoard(Scenario scenario)
    {
        int index = GameController.singleton.players.Count - scenario.minPlayers;
        DataForPlayerCount data = scenario.playerCounts[index];

        ServerCreateTilesForScenario(data);
        ServerCreateStartingBoard(data);

        stateController.ServerChangeState(stateController.tacticSelect);
    }

    #region Private
    [Server]
    private void ServerCreateTilesForScenario(DataForPlayerCount data)
    {
        GameObject tileStack = Instantiate(holderPrefab);
        tileStack.name = "Tile Stack";
        stateController.ServerSpawnObject(tileStack);

        List<GameObject> countrysideTiles = boardFactory.CreateCountrysideStack(data);
        List<GameObject> coreTiles = boardFactory.CreateCoreAndCityStack(data);

        ServerAddTilesToStack(countrysideTiles, tileStack);
        ServerAddTilesToStack(coreTiles, tileStack);

        tileStack.transform.position = new Vector3(-30f, 30f, 0f);
        board.tileStack = tileStack;
    }

    [Server]
    private void ServerAddTilesToStack(List<GameObject> tileList, GameObject tileStack)
    {
        foreach (GameObject tile in tileList)
            tile.transform.SetParent(tileStack.transform);

        //tileStack.GetComponent<NetworkHeirarchySync>().SyncChildren();
    }

    [Server]
    private void ServerCreateStartingBoard(DataForPlayerCount playerCountData)
    {
        var mapShape = MapShapeDatabase.GetScriptableObject(playerCountData.shape.ToString());
        board.tilePositions = new List<HexVector>(mapShape.listOfTilePositions);

        ServerAddPortalTileToBoard(mapShape);
        ServerAddStartingCountryside(mapShape);
    }

    [Server]
    private void ServerAddPortalTileToBoard(MapShape mapShape)
    {
        GameObject portalTile = boardFactory.CreateStartTile(mapShape);
        portalTile.transform.GetChild(3).tag = "PortalTile";
        board.tilePositions.RemoveAt(0);

        portalTile.transform.SetParent(board.transform);
        board.boardHeirarchy.ServerSyncChild(portalTile);
    }

    [Server]
    private void ServerAddStartingCountryside(MapShape mapShape)
    {
        for (int i = 1; i < mapShape.startingCountryside; i++)
        {
            board.ServerPlaceNewTile();
        }
    }
    #endregion
}
